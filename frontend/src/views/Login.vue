<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const email = ref('');
const password = ref('');
const error = ref('');
const passwordError = ref<string | null>(null); 

const validatePassword = () => {
  const senha = password.value;
  const isValida = /^[a-zA-Z0-9]*$/.test(senha);
  const tamanhoValido = senha.length >= 8 && senha.length <= 20;

  if (senha && (!isValida || !tamanhoValido)) {
    passwordError.value =
      'The password must be 8-20 characters long and contain only letters and numbers.';
  } else {
    passwordError.value = null;
  }
};

const handleLogin = async () => {
  try {
    error.value = '';
    validatePassword(); 
    if (passwordError.value) {
      throw new Error(passwordError.value);
    }

    await authStore.login(email.value, password.value);
    router.push('/');
  } catch (e: any) {
    error.value = e.message;
  }
};
</script>

<template>
  <div class="max-w-md mx-auto">
    <div class="content-box">
      <h2 class="text-2xl font-bold text-black mb-6">Login</h2>

      <form @submit.prevent="handleLogin" class="space-y-4">
        <div>
          <label for="email" class="block text-sm text-black font-medium">Email</label>
          <input
            id="email"
            v-model="email"
            type="email"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 text-gray-900"
          />
        </div>

        <div>
          <label for="password" class="block text-sm text-black font-medium">Password</label>
          <input
            id="password"
            v-model="password"
            @input="validatePassword"
            type="password"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 text-gray-900"
          />
          <span v-if="passwordError" class="text-red-600 text-sm">
            {{ passwordError }}
          </span>
        </div>

        <div v-if="error" class="text-red-300 text-sm">{{ error }}</div>

        <button
          type="submit"
          class="w-full flex text-black justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium bg-[#a68ba8] hover:bg-[#815a83] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          Login
        </button>
      </form>
    </div>
  </div>
</template>
