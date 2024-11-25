<script setup lang="ts">
import { useAuthStore } from '../stores/auth';
import { computed } from 'vue';
import { Role } from '../types';

const authStore = useAuthStore();
const userRole = computed(() => authStore.currentUser?.role);
</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h2 class="text-2xl font-bold mb-6">Reports</h2>
    
    <div class="bg-white rounded-lg shadow p-6">
      <div v-if="userRole === Role.Student">
        <h3 class="text-xl font-medium mb-4">Your Quiz Reports</h3>
        <!-- Student reports interface will be implemented here -->
      </div>
      
      <div v-else-if="userRole === Role.Professor">
        <h3 class="text-xl font-medium mb-4">Student Performance Reports</h3>
        <!-- Professor reports interface will be implemented here -->
      </div>
    </div>
  </div>
</template>