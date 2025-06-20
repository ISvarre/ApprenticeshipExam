<script setup lang="ts">
import { ShoppingCart, Plus, ArrowLeft, Check } from "lucide-vue-next";
import { ref, computed, onMounted } from 'vue';
import axios from "axios";


interface shopItem {
    id: string;
    name: string;
    bought: boolean;
}

const newItem = ref({ name: '', bought: false });

const shoppingList = ref<shopItem[]>([]);

async function loadShoppingList() {
    const response = await axios.get('/api/shopitems');
    shoppingList.value = response.data;
}

async function addShopping() {
    const response = await axios.post('/api/shopitems', {
        name: newItem.value.name,
        bought: newItem.value.bought
    });
    shoppingList.value.push(response.data);
}

async function removeShopItem(id: string) {
    await axios.delete(`/api/shopitems/${id}`);
    await loadShoppingList();
}

const toggleBoughtStatus = async (id: string) => {
    const itemIndex = shoppingList.value.findIndex(item => item.id === id);
    if (itemIndex !== -1) {
        const item = shoppingList.value[itemIndex];
        try {
            console.log(id)
            await axios.patch(`/api/shopitems/${id.externalId}`, { bought: !item.bought }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            item.bought = !item.bought;
        } catch (error) {
            console.error('Failed to update item status:', error);
            alert('Kunne ikke oppdatere status for varen. Prøv igjen senere.');
        }
    }
};

const newItemName = ref('');
const newItemRequestedBy = ref('');

const pendingItems = computed(() => {
    return shoppingList.value.filter(item => !item.bought);
});

const boughtItems = computed(() => {
    return shoppingList.value.filter(item => item.bought);
});

const itemsRemaining = computed(() => {
    return pendingItems.value.length;
});

const itemsBoughtCount = computed(() => {
    return boughtItems.value.length;
});

const totalItems = computed(() => {
    return shoppingList.value.length;
});

const addNewItem = async () => {
    if (newItemName.value.trim()) {
        try {
            const response = await axios.post('/api/shopitems', {
                name: newItemName.value.trim(),
                bought: false,
            });
            shoppingList.value.push(response.data);
            newItemName.value = '';
        } catch (error) {
            console.error('Failed to add new item:', error);
            alert('Kunne ikke legge til varen. Prøv igjen senere.');
        }
    } else {
        alert('Vennligst skriv inn navnet på varen.');
    }
};

const goBack = () => {
    console.log('Navigating back to previous page (simulated)');
};

onMounted(() => {
    loadShoppingList();
})
</script>

<template>
    <div class="w-full min-h-screen bg-[#e8efff] py-8">
        <div class="flex flex-col max-w-4xl mx-auto gap-8 px-4 sm:px-6 lg:px-8">
            <div class="flex items-center gap-2 mb-4 cursor-pointer text-gray-700 hover:text-gray-900" @click="goBack">
                <ArrowLeft class="w-5 h-5" />
                <a href="/" class="text-base font-medium hover:underline">Tilbake til forsiden</a>
            </div>

            <div class="flex flex-col w-full gap-6 p-6 bg-white rounded-md drop-shadow-xs">
                <!-- Header -->
                <div class="flex flex-col w-full gap-2 border-b border-gray-200 pb-6">
                    <div class="flex gap-3 items-center">
                        <ShoppingCart class="w-7 h-7 text-gray-800" />
                        <h1 class="text-3xl font-semibold">Innkjøpsliste</h1>
                    </div>
                    <p class="text-md text-gray-500">
                        Legg til ønsker for pålegg og andre varer
                    </p>
                </div>

                <div class="flex gap-4 items-center">
                    <input
                        type="text"
                        v-model="newItemName"
                        placeholder="Legg til ønsket vare..."
                        class="flex-grow p-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        @keyup.enter="addNewItem"
                    />
                    <button
                        @click="addNewItem"
                        class="flex items-center gap-2 bg-black text-white px-5 py-3 rounded-md text-md font-semibold hover:bg-gray-800 transition-colors duration-200 shrink-0"
                    >
                        <Plus class="w-5 h-5 inline" />
                        Legg til
                    </button>
                </div>

                <div class="bg-blue-50 border border-blue-100 rounded-md p-4 flex items-center justify-between text-blue-800 font-medium">
                    <p class="text-base">
                        Innkjøpsliste status
                        <span class="block text-sm text-gray-700">
                            {{ itemsRemaining }} varer gjenstår å kjøpe
                        </span>
                    </p>
                    <div class="flex gap-3 text-sm">
                        <span class="inline-block bg-white text-blue-800 px-3 py-1 rounded-full border border-blue-200">
                            {{ itemsBoughtCount }} kjøpt
                        </span>
                        <span class="inline-block bg-white text-blue-800 px-3 py-1 rounded-full border border-blue-200">
                            {{ totalItems }} totalt
                        </span>
                    </div>
                </div>

                <div class="flex flex-col gap-4">
                    <h2 class="text-xl font-semibold text-gray-800">Gjenstående varer</h2>
                    <div v-if="pendingItems.length === 0 && totalItems > 0" class="text-center text-gray-500 mt-2 p-4 bg-gray-50 rounded-md">
                        <p class="text-lg">Alle varer er kjøpt inn! 🎉</p>
                    </div>
                    <div
                        v-for="item in pendingItems"
                        :key="item.id"
                        class="flex items-center justify-between bg-white border border-gray-200 rounded-md p-4 shadow-sm"
                    >
                        <div class="flex items-center gap-4">
                            <div class="w-6 h-6 border-2 border-gray-300 rounded-full flex items-center justify-center cursor-pointer hover:bg-gray-100 transition-colors duration-200" @click="toggleBoughtStatus(item.id)">
                                <Check v-if="item.bought" class="w-4 h-4 text-gray-500" />
                            </div>
                            <div>
                                <p class="text-lg font-medium text-gray-900">{{ item.name }}</p>
                            </div>
                        </div>
                        <span class="text-sm text-gray-500">Ikke kjøpt</span>
                    </div>
                </div>

                <div class="flex flex-col gap-4 mt-6">
                    <h2 class="text-xl font-semibold text-gray-800">Kjøpte varer</h2>
                    <div v-if="boughtItems.length === 0" class="text-center text-gray-500 mt-2 p-4 bg-gray-50 rounded-md">
                        <p class="text-lg">Ingen varer er kjøpt enda.</p>
                    </div>
                    <div
                        v-for="item in boughtItems"
                        :key="item.id"
                        class="flex items-center justify-between bg-green-50 border border-green-200 rounded-md p-4 shadow-sm"
                    >
                        <div class="flex items-center gap-4">
                            <div class="w-6 h-6 bg-green-600 rounded-full flex items-center justify-center cursor-pointer hover:bg-green-700 transition-colors duration-200" @click="toggleBoughtStatus(item.id)">
                                <Check class="w-4 h-4 text-white" />
                            </div>
                            <div>
                                <p class="text-lg font-medium text-green-800">{{ item.name }}</p>
                            </div>
                        </div>
                        <span class="inline-block bg-black text-white text-xs font-medium px-3 py-1 rounded-full">Kjøpt</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
</style>
