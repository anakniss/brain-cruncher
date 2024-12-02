export interface QuizAttempt {
  id: string;
  userId: string;
  quizId: string;
  answers: { questionId: string; selectedAnswer: number }[];
  score: number;
  completedAt: Date;
}

export interface Alternatives { 
  id: number;
  description: string;
  isCorrect: boolean;
  questionID: number;
}


export interface ExamResults {
  id: number;
  correctAnswers: number;
  examId: number;
  userId: number;
  createdAt: Date;
}

export enum ExamType {
  Quiz = 0,
  Exam = 1,
}

export interface Exams {
  id: number;
  title: string;
  createdAt: Date;
  type: ExamType; // 0, 1
  createdByID: number;
 }
 
 export interface Questions {
  id: string;
  text: string;
  examID: number;
  createdAt: Date;
}

export interface UserExams {
  userID: number;
  examID: number;
  createdAt: Date;
} 

export enum Role {
  Admin = 0,
  Professor = 1,
  Student = 2,
}

export interface Users {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
  role: Role;
}

export interface QuestionPlayInterface {
  id: string;
  text: string;
  examID: number;
  alternatives: Alternatives[];
}
