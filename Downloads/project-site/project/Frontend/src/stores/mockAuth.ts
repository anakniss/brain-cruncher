import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Users } from '../types';

export const useAuthStore = defineStore('auth', () => {
  const currentUser = ref<Users>({
    id: 1,
    email: 'professor@example.com',
    firstName: 'John',
    lastName: 'Doe',
    username: 'professor',
    password: 'password123',
    role: 1
  });
  
  const loading = ref(false);
  const error = ref<string | null>(null);

  async function login(email: string, password: string) {
    // Mock implementation
    return Promise.resolve();
  }

  async function register(email: string, password: string, name: string, role: Users['role']) {
    // Mock implementation
    return Promise.resolve();
  }

  async function signOut() {
    // Mock implementation
    return Promise.resolve();
  }

  return {
    currentUser,
    loading,
    error,
    login,
    register,
    signOut
  };
});