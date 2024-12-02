<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';
import { onMounted } from 'vue';

const authStore = useAuthStore();
const router = useRouter();

onMounted(async () => {
  if (!authStore.currentUser) {
    router.push('/');
    return;
  }
  await populateRankings();
});


interface Result {
  userId: number;
  correctAnswers: number;
}


interface Ranking {
  userId: number;
  name: string;
  correctAnswers: number;
}

const rankings = ref<Ranking[]>([]);
  const resultsData = ref<Result[]>([]);


const fetchAllResults = async () => {
  try {
    const response = await fetch('http://localhost:5086/api/Alternative/GetAllExamResultPerUser');
    if (!response.ok) throw new Error('Failed to fetch resultsData');
    resultsData.value = await response.json();
  } catch (e) {
    console.error(e);
  }
};


const getNameById = async (id: number) => {
  try{
    const response = await fetch(`http://localhost:5086/api/User/GetNameById/${id}`);
    if (!response.ok) throw new Error('Failed to fetch user');
    const user = await response.json();
    return user.name;
  }
  catch (e) {
    console.error(e);
  }
}; 

const populateRankings = async () => {
  try {
    await fetchAllResults();

    const tempRankings = await Promise.all(
      resultsData.value.map(async (result) => {
        const name = await getNameById(result.userId);
        return {
          userId: result.userId,
          name: name || "Unknown",
          correctAnswers: result.correctAnswers,
        };
      })
    );
    rankings.value = tempRankings.sort((a, b) => b.correctAnswers - a.correctAnswers);
  } catch (e) {
    console.error("Failed to populate rankings:", e);
  }
};

</script>

<template>
  <div class="max-w-4xl mx-auto">
    <h2 class="text-2xl font-bold mb-6">Student Rankings</h2>
    
    <div class="bg-white rounded-lg shadow overflow-hidden">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-[#ccbdcd]">
          <tr>
            <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Rank</th>
            <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Student</th>
            <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Total Points</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-if="rankings.length === 0">
            <td colspan="4" class="px-6 py-4 text-center bg-[#f2eef2] text-gray-500">
              No ranking data available yet.
            </td>
          </tr>
          <tr v-else v-for="(ranking, index) in rankings" :key="ranking.userId">
            <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ index + 1 }}</td>
            <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ ranking.name }}</td>
            <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ ranking.correctAnswers }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
