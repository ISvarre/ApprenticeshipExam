<script setup lang="ts">
 import { Calendar, Users  } from 'lucide-vue-next';

 let today = new Date();
</script>

<template>
    <header className="bg-white shadow-lg">
      <div className="flex-col gap-4  sm:flex-row md:max-w-7xl flex items-center justify-between mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <div className="flex items-center space-x-3">
            <div className="bg-blue-600 p-2 rounded-lg">
              <Users color="white"  />
            </div>
            <div>
              <h1 className="text-2xl font-bold text-gray-900">CC Lunsjassistent</h1>
              <p className="text-sm text-gray-600">Crayon Consulting Innlandet</p>
            </div>
          </div>
          <div className="flex items-center space-x-2 text-md text-gray-600">
            <Calendar color="gray"  />
            <p>
              {{ today.toLocaleDateString('no-NO', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }) }}
            </p>
          </div>
      </div>
    </header>
</template>

<style scoped>

</style>
