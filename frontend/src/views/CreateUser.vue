<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';
import { Role } from '../types';

const authStore = useAuthStore();
const router = useRouter();


const formData = ref({
  id: 0,
  firstName: '',
  lastName: '',
  username: '',
  email: '',
  password: '',
  role: Role.Student,
});

const error = ref<string | null>(null);
const success = ref<string | null>(null);
const passwordError = ref<string | null>(null); 


onMounted(() => {
  if (!authStore.currentUser || authStore.currentUser.role !== Role.Admin) {
    router.push('/');
  }
});


const validatePassword = () => {
  const senha = formData.value.password;
  const isValida = /^[a-zA-Z0-9]*$/.test(senha);
  const tamanhoValido = senha.length >= 8 && senha.length <= 20;

  if (senha && (!isValida || !tamanhoValido)) {
    passwordError.value =
      'The password must be 8-20 characters long and contain only letters and numbers.';
  } else {
    passwordError.value = null;
  }
};

const handleSubmit = async () => {
  try {
    error.value = null;
    success.value = null;

    
    validatePassword();
    if (passwordError.value) {
      throw new Error(passwordError.value);
    }

    
    if (
      !formData.value.firstName ||
      !formData.value.lastName ||
      !formData.value.username ||
      !formData.value.email ||
      !formData.value.password
    ) {
      throw new Error('All fields are required.');
    }

    const response = await fetch('http://localhost:5086/api/User', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(formData.value),
    });

    if (!response.ok) {
      const errorData = await response.text();
      throw new Error(errorData || 'Failed to create user.');
    }

    success.value = 'User created successfully!';

   
    formData.value = {
      id: 0,
      firstName: '',
      lastName: '',
      username: '',
      email: '',
      password: '',
      role: Role.Student,
    };
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'An error occurred.';
  }
};
</script>

<template>
  <div class="max-w-2xl mx-auto">
    <div class="bg-white rounded-lg shadow p-6">
      <h2 class="text-2xl font-bold mb-6">Create New User</h2>

      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div>
          <label for="firstName" class="block text-sm font-medium text-gray-700">First Name</label>
          <input
            id="firstName"
            v-model="formData.firstName"
            type="text"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          />
        </div>

        <div>
          <label for="lastName" class="block text-sm font-medium text-gray-700">Last Name</label>
          <input
            id="lastName"
            v-model="formData.lastName"
            type="text"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          />
        </div>

        <div>
          <label for="username" class="block text-sm font-medium text-gray-700">Username</label>
          <input
            id="username"
            v-model="formData.username"
            type="text"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          />
        </div>

        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
          <input
            id="email"
            v-model="formData.email"
            type="email"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          />
        </div>

        <div>
          <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
          <input
            id="password"
            v-model="formData.password"
            @input="validatePassword"
            type="password"
            required
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          />
          <span v-if="passwordError" class="text-red-600 text-sm">
            {{ passwordError }}
          </span>
        </div>

        <div>
          <label for="role" class="block text-sm font-medium text-gray-700">Role</label>
          <select
            id="role"
            v-model="formData.role"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
          >
            <option :value="Role.Admin">Admin</option>
            <option :value="Role.Professor">Professor</option>
            <option :value="Role.Student">Student</option>
          </select>
        </div>

        <div v-if="error" class="text-red-600 text-sm">{{ error }}</div>
        <div v-if="success" class="text-green-600 text-sm">{{ success }}</div>

        <button
          type="submit"
          class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-[#a68ba8] hover:bg-[#815a83] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          Create User
        </button>
      </form>
    </div>
  </div>
</template>
