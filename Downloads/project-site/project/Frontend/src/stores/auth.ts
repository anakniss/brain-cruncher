import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Users } from '../types';

export const useAuthStore = defineStore('auth', () => {
  const currentUser = ref<Users | null>(null);
  const loading = ref(false);
  const error = ref<string | null>(null);

  async function login(email: string, password: string) {
    try {
      console.log('Login attempt:', { email });
      loading.value = true;
      error.value = null;
     
      const response = await fetch('http://localhost:5086/api/User/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email,
          password
        })
      });

      console.log('Response status:', response.status);
      const data = await response.json();
      console.log('Response data:', data);

      if (!response.ok) {
        throw new Error(data.message || 'Login failed');
      }

      // Update the current user with the response data
      currentUser.value = {
        id: data.user.id,
        firstName: data.user.firstName,
        lastName: data.user.lastName,
        username: data.user.username,
        email: data.user.email,
        password: '', //we dont want to store the password on the client
        role: data.user.role
      };

      console.log('Login successful:', currentUser.value);
      return currentUser.value;
    } catch (e) {
      console.error('Login error:', e);
      error.value = e instanceof Error ? e.message : 'An error occurred during login';
      throw e;
    } finally {
      loading.value = false;
    }
  }

  async function signOut() {
    try {
      console.log('Signing out user:', currentUser.value?.username);
      currentUser.value = null;
      error.value = null;
      console.log('Sign out successful');
    } catch (e) {
      console.error('Sign out error:', e);
      error.value = e instanceof Error ? e.message : 'An error occurred during sign out';
      throw e;
    }
  }

  return {
    currentUser,
    loading,
    error,
    login,
    signOut
  };
});