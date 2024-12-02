<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';
import type { Questions, Alternatives, Exams } from '../types';
import { ExamType, Role } from '../types';

interface QuizForm {
  exam: Omit<Exams, 'id' | 'createdAt'>;
  questions: (Omit<Questions, 'id' | 'createdAt'> & {
    alternatives: Omit<Alternatives, 'id' | 'questionID'>[];
  })[];
}

const authStore = useAuthStore();
const router = useRouter();
const REQUIRED_QUESTIONS = 2;
const ALTERNATIVES_PER_QUESTION = 3;


onMounted(() => {
  const userRole = authStore.currentUser?.role;
  if (!authStore.currentUser || (userRole !== Role.Admin && userRole !== Role.Professor)) {
    router.push('/');
  }
});

const quizForm = ref<QuizForm>({
  exam: {
    title: '',
    type: ExamType.Quiz,
    createdByID: authStore.currentUser?.id || 0,
  },
  questions: Array.from({ length: REQUIRED_QUESTIONS }, () => ({
    text: '',
    examID: 0,
    alternatives: Array.from({ length: ALTERNATIVES_PER_QUESTION }, () => ({
      description: '',
      isCorrect: false
    }))
  }))
});

const setCorrectAnswer = (questionIndex: number, alternativeIndex: number) => {
  quizForm.value.questions[questionIndex].alternatives.forEach((alt, idx) => {
    alt.isCorrect = idx === alternativeIndex;
  });
};

const handleSubmit = async () => {
  try {
    
    if (!quizForm.value.exam.title.trim()) {
      throw new Error('Quiz title is required');
    }

    if (quizForm.value.questions.some(q => !q.text.trim())) {
      throw new Error('All questions must have text');
    }

    if (quizForm.value.questions.some(q => 
      q.alternatives.some(a => !a.description.trim()) || 
      !q.alternatives.some(a => a.isCorrect)
    )) {
      throw new Error('All alternatives must have text and each question must have one correct answer');
    }

    quizForm.value.questions.forEach((question, questionIndex) => {
      console.log(`Question ${questionIndex + 1}: ${question.text}`);
      question.alternatives.forEach((alt, altIndex) => {
        console.log(`  Alternative ${altIndex + 1}: ${alt.description}, Correct: ${alt.isCorrect}`);
      });
    });

    
    const examResponse = await fetch('http://localhost:5086/api/Exam', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        title: quizForm.value.exam.title,
        type: ExamType.Quiz,
        createdAt: new Date().toISOString(),
        createdByID: authStore.currentUser?.id
      })
    });

    if (!examResponse.ok) {
      const errorText = await examResponse.text();
      console.error('Exam creation error:', errorText);
      throw new Error('Failed to create exam');
    }

    const exam = await examResponse.json();

    
    quizForm.value.questions.forEach(question => {
      question.examID = exam.id;
    });

    
    for (const question of quizForm.value.questions) {
      
      const questionResponse = await fetch('http://localhost:5086/api/Question', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          text: question.text,
          examID: question.examID,
          createdAt: new Date().toISOString()
        })
      });

      if (!questionResponse.ok) {
        const errorText = await questionResponse.text();
        console.error('Question creation error:', errorText);
        throw new Error('Failed to create question');
      }

      const createdQuestion = await questionResponse.json();

      for (const alternative of question.alternatives) {
        const alternativeResponse = await fetch('http://localhost:5086/api/Alternative', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            description: alternative.description,
            isCorrect: alternative.isCorrect,
            questionID: createdQuestion.id
          })
        });

        if (!alternativeResponse.ok) {
          const errorText = await alternativeResponse.text();
          console.error('Alternative creation error:', errorText);
          throw new Error('Failed to create alternative');
        }
      }
    }

    alert('Quiz created successfully!');
    
    
    quizForm.value = {
      exam: {
        title: '',
        type: ExamType.Quiz,
        createdByID: authStore.currentUser?.id || 0,
      },
      questions: Array.from({ length: REQUIRED_QUESTIONS }, () => ({
        text: '',
        examID: 0,
        alternatives: Array.from({ length: ALTERNATIVES_PER_QUESTION }, () => ({
          description: '',
          isCorrect: false
        }))
      }))
    };
  } catch (error) {
    console.error('Error creating quiz:', error);
    alert(error instanceof Error ? error.message : 'Failed to create quiz');
  }
};
</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h2 class="text-2xl font-bold mb-6">Create New Quiz</h2>
    
    <div class="bg-white rounded-lg shadow p-6">
      <form @submit.prevent="handleSubmit" class="space-y-6">
        <div>
          <label for="title" class="block text-sm font-medium text-gray-700">Quiz Title</label>
          <input
            id="title"
            v-model="quizForm.exam.title"
            type="text"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          />
        </div>
        
        <div class="space-y-8">
          <div v-for="(question, questionIndex) in quizForm.questions" 
               :key="questionIndex" 
               class="border p-6 rounded-md bg-gray-50">
            <div class="mb-4">
              <h3 class="text-lg font-medium mb-2">Question {{ questionIndex + 1 }}</h3>
              <input
                v-model="question.text"
                type="text"
                required
                placeholder="Enter your question"
                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
              />
            </div>

            <div class="space-y-3">
              <h4 class="font-medium text-gray-700">Alternatives</h4>
              <div v-for="(alt, altIndex) in question.alternatives" 
                   :key="altIndex" 
                   class="flex items-center space-x-3 bg-white p-3 rounded-md">
                <span class="text-gray-500 w-6">{{ String.fromCharCode(65 + altIndex) }}.</span>
                <input
                  v-model="alt.description"
                  type="text"
                  required
                  :placeholder="`Alternative ${altIndex + 1}`"
                  class="flex-1 rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                />
                <div class="flex items-center">
                  <input
                    type="radio"
                    :name="`correct-${questionIndex}`"
                    :checked="alt.isCorrect"
                    @change="() => setCorrectAnswer(questionIndex, altIndex)"
                    required
                    class="h-4 w-4 text-indigo-600 focus:ring-indigo-500"
                  />
                  <label class="ml-2 text-sm text-gray-600">Correct</label>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <div class="flex justify-end">
          <button
            type="submit"
            class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          >
            Create Quiz
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
