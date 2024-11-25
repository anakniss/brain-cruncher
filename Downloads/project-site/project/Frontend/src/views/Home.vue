<script setup lang="ts">
import { useAuthStore } from '../stores/mockAuth';
import { computed } from 'vue';
import { Role } from '../types';

const authStore = useAuthStore();
const user = computed(() => authStore.currentUser);
</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h1 class="text-4xl font-bold mb-8">Welcome to Quiz Game</h1>
    
    <div v-if="user" class="bg-white rounded-lg shadow p-6">
      <p class="text-xl mb-4">Welcome back, {{ user.firstName }}!</p>
      <p class="text-gray-600">
        {{ user.role ===  Role.Student? 'Ready to test your knowledge?' : 
           user.role === Role.Professor ? 'Create new quizzes for your students!' :
           'Manage users and oversee the platform.' }}
      </p>
    </div>
    
    <div v-else class="bg-white rounded-lg shadow p-6">
      <p class="text-xl mb-4">Please log in to get started!</p>
    </div>
  </div>
</template>