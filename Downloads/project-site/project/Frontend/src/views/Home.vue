<script setup lang="ts">
import { useAuthStore } from '../stores/auth';
import { computed } from 'vue';
import { Role } from '../types';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const user = computed(() => authStore.currentUser);

const navigateTo = (path: string) => {
  router.push(path);
};
</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h1 class="text-4xl font-bold mb-8">Welcome to Quiz Game</h1>
    
    <div v-if="user" class="space-y-8">
      <div class="bg-white rounded-lg shadow p-6">
        <p class="text-xl mb-4">Welcome back, {{ user.firstName }}!</p>
        <p class="text-gray-600 mb-6">
          {{ user.role === Role.Student ? 'Ready to test your knowledge?' : 
             user.role === Role.Professor ? 'Create new quizzes for your students!' :
             'Manage users and oversee the platform.' }}
        </p>

        <!-- Quick Actions -->
        <div class="mt-8">
          <h2 class="text-lg font-semibold mb-4">Quick Actions</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <template v-if="user.role === Role.Professor">
              <button
                @click="navigateTo('/create-quiz')"
                class="flex items-center justify-center p-4 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors"
              >
                <span class="text-lg">Create New Quiz</span>
              </button>
              <button
                @click="navigateTo('/reports')"
                class="flex items-center justify-center p-4 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors"
              >
                <span class="text-lg">View Reports</span>
              </button>
            </template>

            <template v-if="user.role === Role.Student">
              <button
                @click="navigateTo('/play')"
                class="flex items-center justify-center p-4 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors"
              >
                <span class="text-lg">Take a Quiz</span>
              </button>
              <button
                @click="navigateTo('/ranking')"
                class="flex items-center justify-center p-4 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors"
              >
                <span class="text-lg">View Rankings</span>
              </button>
            </template>
          </div>
        </div>
      </div>

      <!-- Recent Activity Section -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold mb-4">Recent Activity</h2>
        <div class="text-gray-600">
          <p>No recent activity to display.</p>
        </div>
      </div>
    </div>
    
    <div v-else class="bg-white rounded-lg shadow p-6">
      <p class="text-xl mb-4">Please log in to get started!</p>
      <button
        @click="navigateTo('/login')"
        class="px-4 py-2 bg-indigo-600 text-white rounded hover:bg-indigo-700 transition-colors"
      >
        Log In
      </button>
    </div>
  </div>
</template>