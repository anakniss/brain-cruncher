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
  <nav class="app-header">
    <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div class="flex h-16 justify-between">
        <div class="flex">
          <div class="flex flex-shrink-0 items-center overflow-hidden">
            <router-link to="/" class="flex items-center space-x-2">
              <img src='../assets/brain-cruncher2.png' alt="Brain Cruncher Logo" class="w-12 h-12 min-w-[48px] min-h-[48px] object-contain" />
              <span class="text-xl font-medium font-fredoka text-white">Brain Cruncher</span>
            </router-link>
          </div>
          <div class="hidden sm:ml-8 sm:flex sm:space-x-8">
            <router-link
              v-for="item in navItems"
              :key="item.path"
              :to="item.path"
              class="inline-flex items-center px-1 pt-1 text-base font-medium font-fredoka text-white hover:text-gray-200 border-b-2 border-transparent hover:border-white transition-colors duration-200"
              :class="{ 'border-white': $route.path === item.path }"
            >
              {{ item.name }}
            </router-link>
          </div>
        </div>

        <div class="hidden sm:ml-6 sm:flex sm:items-center">
          <div v-if="isLoggedIn" class="flex items-center space-x-4">
            <span class="text-base font-medium text-white">Welcome, {{ userName }}</span>
            <button
              @click="handleSignOut"
              class="rounded-md bg-white/10 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-white/20 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-white transition-colors duration-200"
            >
              Sign Out
            </button>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>
