<script setup lang="ts">
import { useAuthStore } from '../stores/auth';
import { computed } from 'vue';
import { Role } from '../types';

const authStore = useAuthStore();
const user = computed(() => authStore.currentUser);
</script>

<template>
  <div class="max-w-2xl mx-auto">
    <div class="bg-[#ded4e2] rounded-lg shadow p-6">
      <h2 class="text-2xl font-bold mb-6">Profile</h2>
      
      <div class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Name</label>
          <p class="mt-1 text-lg">
          {{ 
          user?.firstName + ' ' + user?.lastName
          }}</p>
        </div>
        
        <div>
          <label class="block text-sm font-medium text-gray-700">Username</label>
          <p class="mt-1 text-lg">{{ user?.username }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Email</label>
          <p class="mt-1 text-lg">{{ user?.email }}</p>
        </div>
        
        <div>
          <label class="block text-sm font-medium text-gray-700">Role</label>
          <p class="mt-1 text-lg capitalize">
            {{
              {
                [Role.Student]: 'Student',
                [Role.Professor]: 'Professor',
                [Role.Admin]: 'Admin'
              }[user?.role as Role] 
            }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>