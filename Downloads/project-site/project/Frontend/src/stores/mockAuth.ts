import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Users } from '../types';
import { Role } from '../types';

export const useAuthStore = defineStore('auth', () => {
  const currentUser = ref<Users>({
    id: 1,
    email: 'professor@example.com',
    firstName: 'John',
    lastName: 'Doe',
    username: 'professor',
    password: 'password123',
    role: Role.Professor
  });
  
  const loading = ref(false);
  const error = ref<string | null>(null);

  async function login(email: string, password: string) {
    // Mock implementation
    return Promise.resolve();
  }

  async function register(firstName: string, lastName: string, username: string, email: string, password: string, role: Role) {
    // Mock implementation
    return Promise.resolve();
  }

  async function signOut() {
    currentUser.value = null as unknown as Users;
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