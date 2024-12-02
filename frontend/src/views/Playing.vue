<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth';
import { Role } from '../types';
import { computed } from 'vue';
import { QuestionPlayInterface } from '../types';

const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();

const questions = ref<QuestionPlayInterface[]>([]);
const currentQuestionIndex = ref(0);
const selectedAnswers = ref<Record<string, number>>({});
const loading = ref(true);
const error = ref<string | null>(null);

const quizId = route.params.id as string;


onMounted(async () => {
  if (!authStore.currentUser || authStore.currentUser.role !== Role.Student) {
    router.push('/');
    return;
  }
  
  await fetchQuizQuestions();
});


const fetchQuizQuestions = async () => {
    try {
        console.log("Fetching questions for quiz ID:", quizId);
        const response = await fetch(`http://localhost:5086/api/Question/GetQuestionByExamId/${quizId}`);
        if (!response.ok) throw new Error('Failed to fetch questions');
        const data = await response.json();
        questions.value = data;
        loading.value = false;
        console.log(data);
        const questionsWithAlternatives = await Promise.all(
            data.map(async (question: QuestionPlayInterface) => {
                console.log("Fetching alternatives for question ID:", question.id);
                const alternativesResponse = await fetch(`http://localhost:5086/api/Alternative/GetAlternativeByQuestionId/${question.id}`);
                if (!alternativesResponse.ok) {
                    console.error("Failed to fetch alternatives for question ID:", question.id);
                    throw new Error(`Failed to fetch alternatives for question ${question.id}`);
                }
                const alternatives = await alternativesResponse.json();
                console.log("Alternatives fetched for question", question.id, alternatives);
                return { ...question, alternatives };
            }
        )
        );
        console.log("Questions with alternatives");
        questions.value = questionsWithAlternatives;
        loading.value = false;

    } catch (e) {
        error.value = e instanceof Error ? e.message : 'An error occurred';
        loading.value = false;
    }
};

const currentQuestion = computed(() => questions.value[currentQuestionIndex.value]);

const handleAnswerSelection = (questionId: string, alternativeId: number) => {
  selectedAnswers.value[questionId] = alternativeId;

};

const goToNextQuestion = () => {
  if (currentQuestionIndex.value < questions.value.length - 1) {
    currentQuestionIndex.value++;
  }
};

const goToPreviousQuestion = () => {
  if (currentQuestionIndex.value > 0) {
    currentQuestionIndex.value--;
  }
};

const submitQuiz = async () => {
  try {
    let score = 0;
    Object.values(selectedAnswers.value).forEach((alternativeId) => {
    const selectedAlternative = questions.value
        .flatMap((q) => q.alternatives)
        .find((a) => a.id === alternativeId);

    if (selectedAlternative?.isCorrect) {
        score++;
    }
    });

    const response = await fetch('http://localhost:5086/api/ExamResult', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        examId: parseInt(quizId),
        userId: authStore.currentUser?.id,
        correctAnswers: score,
      })
    });

    if (!response.ok) throw new Error('Failed to submit quiz');

    router.push('/reports');
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Failed to submit quiz';
  }
};

</script>

<template>
  <div class="max-w-3xl mx-auto">
    <div v-if="loading" class="text-center text-white">
      Loading quiz...
    </div>

    <div v-else-if="error" class="text-center text-red-500">
      {{ error }}
    </div>

    <div v-else class="bg-white rounded-lg shadow-lg p-6">
      <div class="mb-6">
        <div class="flex justify-between items-center mb-2">
          <span class="text-sm text-gray-600">Question {{ currentQuestionIndex + 1 }} of {{ questions.length }}</span>
          <span class="text-sm text-gray-600">
            {{ Math.round(((currentQuestionIndex + 1) / questions.length) * 100) }}% Complete
          </span>
        </div>
        <div class="w-full bg-gray-200 rounded-full h-2">
          <div
            class="bg-[#815a83] h-2 rounded-full transition-all duration-300"
            :style="{ width: `${((currentQuestionIndex + 1) / questions.length) * 100}%` }"
          ></div>
        </div>
      </div>

      <!-- Current question -->
      <div v-if="currentQuestion" class="mb-8">
        <h2 class="text-xl font-semibold mb-4">{{ currentQuestion.text }}</h2>
        
        <div class="space-y-3">
          <div
            v-for="alternative in currentQuestion.alternatives"
            :key="alternative.id"
            @click="handleAnswerSelection(currentQuestion.id, alternative.id)"
            class="p-4 rounded-lg border-2 cursor-pointer transition-all duration-200"
            :class="[
              selectedAnswers[currentQuestion.id] === alternative.id
                ? 'border-[#815a83] bg-[#d9cdd9]'
                : 'border-gray-200 hover:border-gray-300 hover:bg-gray-50'
            ]"
          >
            {{ alternative.description }}
          </div>
        </div>
      </div>

      <!-- Navigation buttons -->
      <div class="flex justify-between">
        <button
          @click="goToPreviousQuestion"
          :disabled="currentQuestionIndex === 0"
          class="px-4 py-2 text-sm font-medium text-white rounded-md bg-[#a68ba8] hover:bg-[#815a83] disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Previous
        </button>

        <button
          v-if="currentQuestionIndex < questions.length - 1"
          @click="goToNextQuestion"
          :disabled="!selectedAnswers[currentQuestion?.id]"
          class="px-4 py-2 text-sm font-medium text-white rounded-md bg-[#a68ba8] hover:bg-[#815a83] disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Next
        </button>

        <button
          v-else
          @click="submitQuiz"
          :disabled="!selectedAnswers[currentQuestion?.id]"
          class="px-4 py-2 text-sm font-medium text-white rounded-md bg-[#a68ba8] hover:bg-[#815a83] disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Submit Quiz
        </button>
      </div>
    </div>
  </div>
</template>
