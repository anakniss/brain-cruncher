<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';
import { Role, ExamType } from '../types';

interface Quiz {
  id: number;
  title: string;
  type: ExamType;
  createdAt: string;
  createdById: number;
}


const availableQuizzes = ref<Quiz[]>([]);
const selectedQuizId = ref<number | null>(null);

const authStore = useAuthStore();
const router = useRouter();

onMounted(async () => {
  const userRole = authStore.currentUser?.role;
  if (!authStore.currentUser || userRole !== Role.Student) {
    router.push('/');
    return;
  }
  
  await fetchQuizzes();
});

const fetchQuizzes = async () => {
  try {
    const response = await fetch('http://localhost:5086/api/Exam/GetAllExams');
    if (!response.ok) {
      throw new Error('Failed to fetch quizzes');
    }
    availableQuizzes.value = await response.json();
  } catch (error) {
    console.error('Error fetching quizzes:', error);
  }
};

const getQuizType = (type: ExamType) => {
  return type === ExamType.Quiz ? 'Quiz' : 'Exam';
};

const startQuiz = () => {
  if (selectedQuizId.value) {
    router.push(`/playing/${selectedQuizId.value}`);
  }
};
</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h2 class="text-2xl font-bold mb-6 text-white">Available Quizzes</h2>

    <div class="bg-white rounded-lg shadow p-6">
      <div v-if="availableQuizzes.length === 0" class="text-gray-500">
        No quizzes available at the moment.
      </div>

      <div v-else class="space-y-2">
        <div
          v-for="quiz in availableQuizzes"
          :key="quiz.id"
          :class="[
            'p-4 rounded-lg cursor-pointer transition-colors',
            selectedQuizId === quiz.id
              ? 'bg-[#d9cdd9] border-2 border-[#815a83]'
              : 'bg-gray-50 hover:bg-gray-100 border-2 border-transparent'
          ]"
          @click="selectedQuizId = quiz.id"
        >
          <div class="flex justify-between items-center">
            <span class="font-medium">{{ quiz.title }}</span>
            <span class="text-sm text-gray-600">{{ getQuizType(quiz.type) }}</span>
          </div>
        </div>
      </div>

      <button
        v-if="selectedQuizId"
        @click="startQuiz"
        class="mt-6 w-full text-white py-2 px-4 rounded-md bg-[#a68ba8] hover:bg-[#815a83] transition-colors"
      >
        Start Selected Quiz
      </button>
    </div>
  </div>
</template>