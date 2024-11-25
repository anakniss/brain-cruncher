//
/*
todo 
- change from http to https later, maybe
- maybe delete method if we don't wish to create new users
export interface Users {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
  role: 'admin' | 'professor' | 'student';// 0 , 1 , 2
}
*/

import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Users } from '../types';
import http from 'http'; // Use the built-in http module
import type { Role } from '../types';


export const useAuthStore = defineStore('auth', () => {
  const currentUser = ref<Users | null>(null);
  const loading = ref(false);
  const error = ref<string | null>(null);

  //all checks are done in the backend
  async function register(id : number, firstName: string, lastName: string, username: string, email: string, password: string, role: Role) {
    try {
      loading.value = true;
      error.value = null;
      
      const userData = JSON.stringify({
        id,
        firstName,
        lastName,
        username,
        email,
        password,
        role
      });
      // for options check ../backend/Properties/launchSettings.json && ../backend/Controllers
      const options = {
        hostname: 'localhost',
        port: 5086,
        path: '/api/Users',
        method: 'POST', 
        headers: {
          'Content-Type': 'application/json',
          'Content-Length': Buffer.byteLength(userData)
        },
      };
      const req = http.request(options, (res) => {
        let responseData = '';
        res.on('data', (chunk) => {
          responseData += chunk;
        });
        res.on('end', () => {
          if (res.statusCode === 200) {
            const responseJson = JSON.parse(responseData);
            currentUser.value = {
              id: responseJson.user.id,
              firstName,
              lastName,
              username,
              email,
              password,
              role
            };
          } else {
            error.value = `Error: ${res.statusCode} - ${responseData}`;
            throw new Error(`Error: ${res.statusCode}`);
          }
        });
      });
      
      req.on('error', (e) => {
        error.value = e.message;
        throw e;
      });
      req.write(userData);
      req.end();

    } catch (e: any) {
      error.value = e.message;
      throw e;
    }
    finally {
      loading.value = false;
    }
  }


  async function login(email: string, password: string) {
    try
    {
      loading.value = true;
      error.value = null;
      
      const loginData = JSON.stringify({
        email,
        password
      });
      //sending post request to backend, to check if user login info is valid
      const options = {
        hostname: 'localhost',
        port: 5086,
        path: '/api/Users',
        method: 'POST', 
        headers: {
          'Content-Type': 'application/json',
          'Content-Length': Buffer.byteLength(loginData)
        },
      };
      const req = http.request(options, (res) => {
        let responseData = '';
        res.on('data', (chunk) => {
          responseData += chunk;
        });
        res.on('end', () => {
          if (res.statusCode === 200) {
            const responseJson = JSON.parse(responseData);
            currentUser.value = responseJson.User;
          } else {
            error.value = `Error: ${res.statusCode} - ${responseData}`;
          }
        });
      });
  
      req.on('error', (e) => {
        error.value = e.message;
      });
  
      req.write(loginData);
      req.end();
      
    } catch (e: any) {
      error.value = e.message;
    } finally {
      loading.value = false;
    }
}

async function signOut() {
  try {
    currentUser.value = null; // tries to set the current user to null to sign out
    error.value = null;
  } catch (e: any) {
    error.value = e.message;
  }
}


  return {
    currentUser,
    loading,
    error,
    register,
    login,
    signOut
  };
});