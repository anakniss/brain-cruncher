<script setup lang="ts">
import { useAuthStore } from '../stores/auth';
import { computed } from 'vue';

const authStore = useAuthStore();
const user = computed(() => authStore.currentUser);
</script>

<template>
  <div class="max-w-2xl mx-auto">
    <div class="bg-white rounded-lg shadow p-6">
      <h2 class="text-2xl font-bold mb-6">Profile</h2>
      
      <div class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Name</label>
          <p class="mt-1 text-lg">{{ user?.firstName }}</p>
        </div>
        
        <div>
          <label class="block text-sm font-medium text-gray-700">Email</label>
          <p class="mt-1 text-lg">{{ user?.lastName }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Email</label>
          <p class="mt-1 text-lg">{{ user?.email }}</p>
        </div>
        
        <div>
          <label class="block text-sm font-medium text-gray-700">Role</label>
          <p class="mt-1 text-lg capitalize">{{ user?.role }}</p>
        </div>
      </div>
    </div>
  </div>
</template>