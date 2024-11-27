<script setup lang="ts">
import { useAuthStore } from '../stores/auth';
import { computed } from 'vue';
import { useRouter } from 'vue-router';
import { Role } from '../types';

const authStore = useAuthStore();
const router = useRouter();

const isLoggedIn = computed(() => !!authStore.currentUser);
const userRole = computed(() => authStore.currentUser?.role);
const userName = computed(() => authStore.currentUser?.username);

const navItems = computed(() => {
  const items = [
    { name: 'Home', path: '/' }
  ];

  if (isLoggedIn.value) {
    items.push({ name: 'Profile', path: '/profile' });
    items.push({ name: 'Ranking', path: '/ranking' });
    items.push({ name: 'Reports', path: '/reports' });

    // Allow both professors and admins to create quizzes
    if (userRole.value === Role.Professor || userRole.value === Role.Admin) {
      items.push({ name: 'Create Quiz', path: '/create-quiz' });
    }

    // Only admins can create users
    if (userRole.value === Role.Admin) {
      items.push({ name: 'Create User', path: '/create-user' });
    }

    if (userRole.value === Role.Student) {
      items.push({ name: 'Play', path: '/play' });
    }
  } else {
    items.push({ name: 'Login', path: '/login' });
  }

  return items;
});

const handleSignOut = async () => {
  await authStore.signOut();
  router.push('/login');
};
</script>

<template>
  <nav class="bg-white shadow-lg">
    <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div class="flex h-16 justify-between">
        <div class="flex">
          <div class="flex flex-shrink-0 items-center">
            <router-link to="/" class="text-xl font-bold text-indigo-600">
              QuizGame
            </router-link>
          </div>
          <div class="hidden sm:ml-8 sm:flex sm:space-x-8">
            <router-link
              v-for="item in navItems"
              :key="item.path"
              :to="item.path"
              class="inline-flex items-center px-1 pt-1 text-sm font-medium text-gray-900 hover:text-indigo-600 border-b-2 border-transparent hover:border-indigo-600 transition-colors duration-200"
              :class="{ 'border-indigo-600': $route.path === item.path }"
            >
              {{ item.name }}
            </router-link>
          </div>
        </div>

        <div class="hidden sm:ml-6 sm:flex sm:items-center">
          <div v-if="isLoggedIn" class="flex items-center space-x-4">
            <span class="text-sm text-gray-700">Welcome, {{ userName }}</span>
            <button
              @click="handleSignOut"
              class="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 transition-colors duration-200"
            >
              Sign Out
            </button>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>