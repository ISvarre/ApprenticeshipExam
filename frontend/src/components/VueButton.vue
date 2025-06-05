<script setup lang="ts">
console.log('Button')
</script>

<template>
  <div class="flex flex-col items-center justify-center text-center w-full px-10 py-6 min-h-full h-screen">
    <h1 class="grid grid-cols-5 gap-4 px-20 bg-red-500">Hello</h1>
  </div>

</template>

<style scoped>

</style>

<script setup lang="ts">
import { Utensils, PencilLine, XCircle, CheckCircle } from "lucide-vue-next";
import { ref, watch } from 'vue';

// Define the structure for a menu item
interface MenuItem {
    day: string;
    mainDish: string;
    description: string;
    price: string;
    highlighted: boolean;
    isEditing?: boolean; // New property to track if an item is currently being edited
}

const menuItems = ref<MenuItem[]>([
    {
        day: 'I dag',
        mainDish: 'Kylling Tikka Masala',
        description: 'Med basmatiris og nanbrød',
        price: '149,-',
        highlighted: true,
    },
    {
        day: 'Mandag',
        mainDish: 'Taco',
        description: 'Med kjøtt, salat og tilbehør',
        price: '149,-',
        highlighted: false,
    },
    {
        day: 'Tirsdag',
        mainDish: 'Indrefilet',
        description: 'Med potetmos og grønnsaker',
        price: '189,-',
        highlighted: false,
    },
    {
        day: 'Onsdag', // Added Wednesday as it's common
        mainDish: 'Pannekaker',
        description: 'Med syltetøy og sukker',
        price: '99,-',
        highlighted: false,
    },
    {
        day: 'Torsdag',
        mainDish: 'Fiskegrateng',
        description: 'Med salat og brød',
        price: '139,-',
        highlighted: false,
    },
    {
        day: 'Fredag',
        mainDish: 'Pizza',
        description: 'Dagens pizza med salat',
        price: '129,-',
        highlighted: false,
    },
]);

const isEditingMenu = ref(false);
const originalMenuItems = ref<MenuItem[]>([]); // To store a copy for cancellation

// Watch for changes to menuItems when editing starts to save original state
watch(isEditingMenu, (newValue) => {
    if (newValue) {
        originalMenuItems.value = JSON.parse(JSON.stringify(menuItems.value)); // Deep copy
        // Set all items to not highlighted when editing to avoid confusion
        menuItems.value.forEach(item => item.highlighted = false);
    } else {
        // When editing stops, find "I dag" (or the current day) and highlight it
        const today = new Date().toLocaleDateString('no-NO', { weekday: 'long' });
        const todayIndex = menuItems.value.findIndex(item => item.day.toLowerCase() === today.toLowerCase());

        menuItems.value.forEach((item, index) => {
            item.highlighted = (index === todayIndex); // Highlight today's menu item
            item.isEditing = false; // Ensure editing state is off for all items
        });

        // If "I dag" exists as an actual menu item, then mark that
        const todayMenuItem = menuItems.value.find(item => item.day === 'I dag');
        if (todayMenuItem) {
            todayMenuItem.highlighted = true;
        } else {
            // If "I dag" specific item is not defined, let's keep the first item highlighted for now
            // This is a simplification; in a real app, you'd calculate current day from `new Date()`
            if (menuItems.value.length > 0) {
                menuItems.value[0].highlighted = true;
            }
        }
    }
}, { immediate: true }); // Run immediately on component load to set initial highlight


const toggleEditMode = () => {
    isEditingMenu.value = !isEditingMenu.value;
    // When entering edit mode, set isEditing to true for all items
    // When exiting, it will be handled by the watch effect
    if (isEditingMenu.value) {
        menuItems.value.forEach(item => item.isEditing = true);
    }
};

const saveMenu = () => {
    // In a real application, you'd send `menuItems.value` to your backend here.
    console.log('Menu saved:', menuItems.value);
    isEditingMenu.value = false;
};

const cancelEdit = () => {
    menuItems.value = JSON.parse(JSON.stringify(originalMenuItems.value)); // Revert to original
    isEditingMenu.value = false;
};

// Function to handle "I dag" text to actual day based on current date
const getCurrentDayName = () => {
    const days = ['Søndag', 'Mandag', 'Tirsdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lørdag'];
    return days[new Date().getDay()];
};

// Update "I dag" item's 'day' property to match current day when editing starts.
// This ensures that when someone edits the menu, they know which 'day' "I dag" corresponds to.
// This is a simple implementation. For production, consider robust date management.
const updateTodayDay = () => {
    const todayItem = menuItems.value.find(item => item.day === 'I dag');
    if (todayItem) {
        todayItem.day = getCurrentDayName();
    }
}
</script>

<template>
    <div class="flex flex-col bg-white w-full p-6 rounded-md shadow-sm">
        <!-- Header -->
        <div class="flex items-center gap-2 mb-2 shrink-0">
            <Utensils class="w-5 h-5 text-gray-800" />
            <h1 class="text-2xl font-semibold text-gray-800">Kantinemeny i dag</h1>
        </div>
        <p class="text-sm text-gray-500 mb-6">Se hva som serveres i kantina</p>

        <!-- Menu Items List -->
        <div class="flex flex-col gap-4 mb-6">
            <div
                v-for="(item, index) in menuItems"
                :key="index"
                :class="[
          'flex flex-col p-4 rounded-md border border-gray-200 shadow-sm',
          item.highlighted && !isEditingMenu ? 'bg-blue-50 border-blue-200' : 'bg-white',
        ]"
            >
                <!-- Display Mode -->
                <div v-if="!isEditingMenu" class="flex justify-between items-center">
                    <div class="flex flex-col">
                        <p
                            :class="[
                'text-lg font-semibold',
                item.highlighted ? 'text-blue-700' : 'text-gray-900',
              ]"
                        >
              <span
                  :class="{ 'font-bold': item.highlighted, 'text-blue-700': item.highlighted }"
              >{{ item.day }}:</span
              >
                            {{ item.mainDish }}
                        </p>
                        <p class="text-sm text-gray-600">
                            {{ item.description }}
                        </p>
                    </div>
                    <div
                        :class="[
              'inline-block px-4 py-2 rounded-full font-semibold text-gray-800 text-sm',
              item.highlighted ? 'bg-blue-200' : 'bg-gray-100',
            ]"
                    >
                        {{ item.price }}
                    </div>
                </div>

                <!-- Edit Mode -->
                <div v-else class="flex flex-col gap-2">
                    <div class="flex items-center gap-2">
                        <input
                            v-model="item.day"
                            type="text"
                            class="flex-1 p-2 border border-gray-300 rounded-md text-sm focus:outline-none focus:ring-1 focus:ring-blue-500"
                            placeholder="Dag (f.eks. Mandag)"
                            :disabled="item.day === 'I dag' && isEditingMenu"
                        />
                        <input
                            v-model="item.mainDish"
                            type="text"
                            class="flex-[2] p-2 border border-gray-300 rounded-md text-sm font-semibold focus:outline-none focus:ring-1 focus:ring-blue-500"
                            placeholder="Hovedrett"
                        />
                        <input
                            v-model="item.price"
                            type="text"
                            class="w-24 p-2 border border-gray-300 rounded-md text-sm text-right focus:outline-none focus:ring-1 focus:ring-blue-500"
                            placeholder="Pris (f.eks. 149,-)"
                        />
                    </div>
                    <input
                        v-model="item.description"
                        type="text"
                        class="w-full p-2 border border-gray-300 rounded-md text-sm focus:outline-none focus:ring-1 focus:ring-blue-500"
                        placeholder="Beskrivelse (f.eks. Med ris og nanbrød)"
                    />
                </div>
            </div>
        </div>

        <!-- Edit/Save/Cancel Buttons -->
        <div class="flex flex-col gap-3">
            <button
                v-if="!isEditingMenu"
                @click="toggleEditMode"
                class="w-full py-3 px-4 bg-gray-100 text-gray-800 font-medium rounded-md border border-gray-300 hover:bg-gray-200 transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-gray-400 focus:ring-offset-2 flex items-center justify-center gap-2"
            >
                <PencilLine class="w-4 h-4" />
                Oppdater meny
            </button>

            <div v-else class="flex justify-end gap-3">
                <button
                    @click="cancelEdit"
                    class="flex-1 py-2 px-4 bg-red-500 text-white font-medium rounded-md hover:bg-red-600 transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 flex items-center justify-center gap-2"
                >
                    <XCircle class="w-4 h-4" />
                    Avbryt
                </button>
                <button
                    @click="saveMenu"
                    class="flex-1 py-2 px-4 bg-green-500 text-white font-medium rounded-md hover:bg-green-600 transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2 flex items-center justify-center gap-2"
                >
                    <CheckCircle class="w-4 h-4" />
                    Lagre endringer
                </button>
            </div>
        </div>
    </div>
</template>

<style scoped>
/* No custom styles needed beyond Tailwind */
</style>
