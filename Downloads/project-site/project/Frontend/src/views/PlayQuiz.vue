<script setup lang="ts">
import { ref } from 'vue';

// Placeholder for quiz list
const availableQuizzes = ref([]);
const selectedQuiz = ref(null);
</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h2 class="text-2xl font-bold mb-6">Play Quiz</h2>
    
    <div v-if="!selectedQuiz" class="bg-white rounded-lg shadow p-6">
      <h3 class="text-xl font-medium mb-4">Available Quizzes</h3>
      
      <div v-if="availableQuizzes.length === 0" class="text-gray-500">
        No quizzes available at the moment.
      </div>
      
      <div v-else class="space-y-4">
        <!-- Quiz list will be implemented here -->
      </div>
    </div>
    
    <div v-else class="bg-white rounded-lg shadow p-6">
      <!-- Quiz playing interface will be implemented here -->
    </div>
  </div>
</template>