<script setup lang="ts">
import { ref } from 'vue';
import type { Questions, Alternatives } from '../types'; // Import types


const title = ref<string>('');


const Questionss = ref<(Partial<Questions> & { alternatives: Alternatives[] })[]>([]);

//need to change to make http request to generate exam id
const addQuestions = () => {
  Questionss.value.push({
    id: Date.now().toString(),
    text: '',
    examID: 0, 
    createdAt: new Date(),
    alternatives: [
      { id: 1, description: '', isCorrect: false, questionID: Date.now() },
      { id: 2, description: '', isCorrect: false, questionID: Date.now() },
      { id: 3, description: '', isCorrect: false, questionID: Date.now() },
      { id: 4, description: '', isCorrect: false, questionID: Date.now() },
    ]
  });
};


const removeQuestions = (index: number) => {
  Questionss.value.splice(index, 1);
};
</script>
<template>
  <div class="max-w-4xl mx-auto">
    <h2 class="text-2xl font-bold mb-6">Create New Quiz</h2>
    
    <div class="bg-white rounded-lg shadow p-6">
      <form @submit.prevent class="space-y-6">
        <div>
          <label for="title" class="block text-sm font-medium text-gray-700">Quiz Title</label>
          <input
            id="title"
            v-model="title"
            type="text"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          />
        </div>
        
        <div class="space-y-4">
          <div v-for="(question, index) in Questionss" :key="index" class="border p-4 rounded-md">
            <div class="flex justify-between items-start mb-4">
              <h3 class="text-lg font-medium">Question {{ index + 1 }}</h3>
              <button
                @click="removeQuestions(index)"
                class="text-red-600 hover:text-red-800"
              >
                Remove
              </button>
            </div>
            
            <div class="space-y-4">
              <div>
                <label class="block text-sm font-medium text-gray-700">Question Text</label>
                <input
                  v-model="question.text"
                  type="text"
                  required
                  class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700">Exam ID</label>
                <input
                  v-model="question.examID"
                  type="number"
                  required
                  class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                />
              </div>

              <div class="space-y-2">
                <div v-for="(alt, optionIndex) in question.alternatives" :key="alt.id" class="flex items-center">
                  <input
                    v-model="alt.description"
                    type="text"
                    required
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                    :placeholder="`Option ${optionIndex + 1}`"
                  />
                  <input
                    type="radio"
                    :name="`correct-${index}`"
                    :value="alt.id"
                    v-model="question.alternatives.find(a => a.isCorrect)?.id"
                    class="ml-2"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <div class="flex justify-between">
          <button
            type="button"
            @click="addQuestions"
            class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700"
          >
            Add Question
          </button>
          
          <button
            type="submit"
            class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-green-600 hover:bg-green-700"
          >
            Save Quiz
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
 