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
    <h1
      class="text-4xl flex flex-col items-center justify-center text-center font-bold mb-8 text-white"
    >
      Welcome to Brain Cruncher
    </h1>

    <div v-if="user" class="space-y-8">
      <div class="content-box">
        <p class="text-xl text-black mb-4">Welcome back, {{ user.firstName }}!</p>
        <p class="text-black mb-6">
          {{
            user.role === Role.Student
              ? 'Ready to test your knowledge?'
              : user.role === Role.Professor
              ? 'Create new quizzes for your students!'
              : 'Manage users and oversee the platform.'
          }}
        </p>

        <div class="mt-8">
          <h2 class="text-lg text-black font-semibold mb-4">Quick Actions</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <template v-if="user.role === Role.Professor || user.role === Role.Admin">
              <button
                @click="navigateTo('/create-quiz')"
                class="flex items-center justify-center p-4 bg-[#a68ba8] text-white rounded-lg hover:bg-[#815a83] transition-colors"
              >
                <span class="text-lg">Create New Quiz</span>
              </button>
              <button
                @click="navigateTo('/reports')"
                class="flex items-center justify-center p-4 bg-[#a68ba8] text-white rounded-lg hover:bg-[#815a83] transition-colors"
              >
                <span class="text-lg">View Reports</span>
              </button>
            </template>

            <template v-if="user.role === Role.Student">
              <button
                @click="navigateTo('/play')"
                class="flex items-center justify-center p-4 bg-[#a68ba8] text-white rounded-lg hover:bg-[#815a83] transition-colors"
              >
                <span class="text-lg">Take a Quiz</span>
              </button>
              <button
                @click="navigateTo('/ranking')"
                class="flex items-center justify-center p-4 bg-[#a68ba8] text-white rounded-lg hover:bg-[#815a83] transition-colors"
              >
                <span class="text-lg">View Rankings</span>
              </button>
            </template>
          </div>
        </div>
      </div>
    </div>

    <div
      v-else
      class="content-box flex flex-col items-center justify-center text-center"
    >
      <p class="text-xl text-black mb-4">Please log in to get started!</p>
      <button
        @click="navigateTo('/login')"
        class="w-1/3 flex text-black justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium bg-[#a68ba8] hover:bg-[#815a83] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
      >
        Log In
      </button>
    </div>
  </div>
</template>
