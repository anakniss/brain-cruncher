<script setup lang="ts">
import { useAuthStore } from '../stores/auth';
import { computed, onMounted, ref } from 'vue';
import { Role, ExamResults } from '../types';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const userRole = computed(() => authStore.currentUser?.role);
const userId = authStore.currentUser?.id || -1;
const reportAllUsers = ref<Report[]>([]);
const reportCurrentUser = ref<Report | null>(null);
interface Report {
  userId: number;
  name: string;
  totalScore: number;
  examResults: ExamResults[];
  createdAt: string;
  completedQuizCount: number;
}

const isModalOpen = ref(false);
const selectedExamResults = ref<ExamResults[]>([]);

const openModal = (examResults: ExamResults[]) => {
  selectedExamResults.value = examResults;
  isModalOpen.value = true;
  console.log('Modal opened with results:', examResults);
};

const closeModal = () => {
  isModalOpen.value = false;
  selectedExamResults.value = [];
};

onMounted(async () => {
  if (authStore.currentUser?.role === Role.Student) {
    await setCurrentUserReports();
  }
  else if (authStore.currentUser?.role === Role.Professor || authStore.currentUser?.role === Role.Admin) {
    await setAllUserReports();
  }
  else {
    router.push('/');
  }
});


const fetchAllExamsResultsFromOneStudent = async (id: number) => {
  try {
    const response = await fetch(`http://localhost:5086/api/ExamResult/GetExamResultByUser/${id}`);
    if (!response.ok) throw new Error('Failed to fetch exams from one student');
    return await response.json();
  } catch (e) {
    console.error(e);
  }
};

const fetchUserTotalScore = async (id:number) => {
  try {
    const response = await fetch(`http://localhost:5086/api/Alternative/GetTotalCorrectAnswersForUser/${id}`);
    if (!response.ok) throw new Error('Failed to fetch total score');
    const data = await response.json();
    return data.correctAnswers;
  } catch (e) {
    console.error(e);
  }
};

const fetchUserCompletedExamCount = async (id:number) => {
  try{
    const response = await fetch(`http://localhost:5086/api/ExamResult/GetExamCountByUserId/${id}`);
    if (!response.ok) throw new Error('Failed to fetch completed exam count');
    return await response.json();
  } catch (e) {
    console.error(e);
  }
}

const DateOptions = { 
  year: 'numeric' as const, 
  month: 'numeric' as const, 
  day: 'numeric' as const, 
  hour: '2-digit' as const, 
  minute: '2-digit' as const, 
  second: '2-digit' as const 
};

const getNameById = async (userId: number) => {
  try{
    const response = await fetch(`http://localhost:5086/api/User/GetNameById/${userId}`);
    if (!response.ok) throw new Error('Failed to fetch user');
    const user = await response.json();
    return user.name;
  }
  catch (e) {
    console.error(e);
  }
};

const getUserReport = async (userId:number) => {
  try{
    const examResults  = await fetchAllExamsResultsFromOneStudent(userId);
    const totalScore = await fetchUserTotalScore(userId);
    const completedQuizCount = await fetchUserCompletedExamCount(userId);
    const name = await getNameById(userId);
    const createdAt = new Date().toLocaleDateString('pt-BR',DateOptions);
    const dataReturn = {
      userId: userId || 0,
      name: name || "",
      totalScore: totalScore || 0,
      examResults: examResults || [],
      createdAt: createdAt || "",
      completedQuizCount: completedQuizCount || 0
    };
    return dataReturn;
  } catch (e) {
    console.error(e);
  }
}

const fetchAllStudentIds = async () => {
  try {
    const response = await fetch('http://localhost:5086/api/User/GetStudentIds');
    if (!response.ok) throw new Error('Failed to fetch student ids');
    const ids = response.json();
    return ids;
  } catch (e) {
    console.error(e);
  }
};

const setCurrentUserReports = async () => {
  try{
    const response = await getUserReport(userId);
    if(response){
      reportCurrentUser.value = response;
    }
    console.log(reportCurrentUser.value);
  } catch (e) {
    console.error(e);
  }
}

const setAllUserReports = async () => {
  try {
    const studentIds = await fetchAllStudentIds();
    const reports = await Promise.all(
      studentIds.map(async (id: number) => {
        return await getUserReport(id);
      })
    );
    console.log(reports);
    reportAllUsers.value = reports;
  } catch (e) {
    console.error(e);
  }
}
</script>

<template>
  <div class="max-w-full mx-auto">
    <h2 class="text-2xl font-bold mb-6">Reports</h2>

    <div class="bg-white rounded-lg shadow overflow-hidden">
      <div v-if="userRole === Role.Student">
        <h3 class="text-xl font-medium mb-4">Your Reports</h3>
        <table class="min-w-full divide-y text-black divide-gray-200">
          <thead class="bg-[#ccbdcd]">
            <tr>
              <th class="px-6 py-3 text-center text-sm font-medium text-black uppercase tracking-wider">Id</th>
              <th class="px-6 py-3 text-center text-sm font-medium text-black uppercase tracking-wider">Name</th>
              <th class="px-6 py-3 text-center text-sm font-medium text-black uppercase tracking-wider">Total Points</th>
              <th class="px-6 py-3 text-center text-sm font-medium text-black uppercase tracking-wider">Report Creation Date</th>
              <th class="px-6 py-3 text-center text-sm font-medium text-black uppercase tracking-wider">Completed Quizzes</th>
              <th class="px-6 py-3 text-center text-sm font-medium text-black uppercase tracking-wider">Exam Results</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ reportCurrentUser?.userId }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ reportCurrentUser?.name }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ reportCurrentUser?.totalScore }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ reportCurrentUser?.createdAt }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ reportCurrentUser?.completedQuizCount }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">
                <button 
                  @click="openModal(reportCurrentUser?.examResults || [])"
                  class="bg-[#a68ba8] hover:bg-[#815a83] text-white font-bold py-2 px-4 rounded"
                >
                  See all exam results
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-else>
        <h3 class="text-xl font-medium mb-4">All Students' Reports</h3>
        <table class="min-w-full divide-y text-black bg-[#d9cdd9] divide-gray-200">
          <thead class="bg-[#ccbdcd]">
            <tr>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Id</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Name</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Total Points</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Report Creation Date</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Completed Quizzes</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Exam Results</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="report in reportAllUsers" :key="report.userId">
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ report.userId }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ report.name }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ report.totalScore }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ report.createdAt }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ report.completedQuizCount }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">
                <button 
                  @click="openModal(report.examResults)"
                  class="bg-[#a68ba8] hover:bg-[#815a83] text-white font-bold py-2 px-4 rounded"
                >
                  See all exam results
                </button>
              </td>
            </tr>
          </tbody>
        </table>        
      </div>
    </div>

    <div v-if="isModalOpen" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white p-6 rounded-lg shadow-lg max-w-4xl w-full max-h-[80vh] overflow-y-auto">
        <div class="flex justify-between items-center mb-4">
          <h3 class="text-xl font-bold">Exam Results</h3>
          <button @click="closeModal" class="text-gray-500 hover:text-gray-700">
            <span class="text-2xl">&times;</span>
          </button>
        </div>
        
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-[#ccbdcd]">
            <tr>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">ID</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Exam ID</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Correct Answers</th>
              <th class="px-6 py-3 text-center text-xs font-medium text-black uppercase tracking-wider">Created At</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="result in selectedExamResults" :key="result.id">
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ result.id }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ result.examId || 0 }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ result.correctAnswers }}</td>
              <td class="px-6 py-4 bg-[#f2eef2] text-center whitespace-nowrap">{{ new Date(result.createdAt).toLocaleDateString('pr-BR', DateOptions) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  background-color: rgba(0, 0, 0, 0.5);
}
</style>
