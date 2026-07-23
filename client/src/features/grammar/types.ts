export type MasteryLevel = 'not_started' | 'learning' | 'practicing' | 'strong'

export type ImportMode = 'create' | 'append' | 'replace'

export interface TopicTreeNode {
  slug: string
  title: string
  sortOrder: number
  exerciseCount: number
  masteryLevel: MasteryLevel
  accuracyPct: number
  children: TopicTreeNode[]
}

export interface TopicDetail {
  slug: string
  title: string
  explanationMd: string
  exerciseCount: number
  totalAttempts: number
  totalCorrect: number
  accuracyPct: number
  masteryLevel: MasteryLevel
  weakExerciseCount: number
}

export interface ImportTopic {
  slug: string
  parentSlug?: string | null
  title: string
  explanationMd: string
  sortOrder: number
}

export interface ImportSlot {
  label?: string | null
  accepted: string[]
  distractors: string[]
}

export interface ImportExercise {
  prompt: string
  notes?: string | null
  slots: ImportSlot[]
}

export interface ImportRequest {
  importMode: ImportMode
  topic: ImportTopic
  exercises: ImportExercise[]
}

export interface ImportResponse {
  topicId: number
  exercisesCreated: number
  exercisesTotal: number
  slotsCreated: number
  warnings: string[]
}

export interface TopicProgressRow {
  slug: string
  title: string
  exerciseCount: number
  masteryLevel: MasteryLevel
  accuracyPct: number
  depth: number
}
