////////////////////////////////////

// 更新數量
// 設置上下限
const uom_min_limit = 1; // 最小數量
const uom_max_limit = 10; // 最大數量

// 更新按鈕狀態的通用方法
function updateButtonStates(component) {
    const input = $(component).find('.uom-input');
    const reduce_btn = $(component).find('.reduce-btn');
    const add_btn = $(component).find('.add-btn');
    const current_value = parseInt(input.val(), 10);

    // 更新按鈕狀態
    reduce_btn.toggleClass('disabled', current_value <= uom_min_limit);
    add_btn.toggleClass('disabled', current_value >= uom_max_limit);
}

// 初始化按鈕狀態
$('.uom-component').each(function () {
    updateButtonStates(this);
});

// 處理數量的變化（減少跟增加）
$('.reduce-btn, .add-btn').on('click', function () {
    const component = $(this).closest('.uom-component');
    const input = component.find('.uom-input');
    const current_value = parseInt(input.val(), 10);

    // 根據點擊的按鈕決定數量加減
    let new_value = current_value + ($(this).hasClass('add-btn') ? 1 : -1);

    // 檢查是否超過上下限
    if (new_value >= uom_min_limit && new_value <= uom_max_limit) {
        input.val(new_value);
        updateButtonStates(component); // 更新按鈕狀態
    }
});





////////////////////////////////////////////// 味道分布-極座標圖 //////////////////////////////////////////////
const ctx = $("#polarAreaChart")[0].getContext("2d");
const polarAreaChart = new Chart(ctx, {
    type: 'polarArea',
    data: {
        labels: ['Fragrant', 'Sour', 'Bitter', 'Sweet', 'Strong'],
        datasets: [{
            label: '咖啡豆風味等級',
            data: [], // 每個屬性的等級 (範圍 1-5)
            backgroundColor: [
                'rgba(41, 52, 80, 0.6)',   // 深藍灰色
                'rgba(74, 144, 226, 0.6)', // 明亮藍色
                'rgba(43, 214, 109, 0.6)', // 柔亮綠色
                'rgba(127, 239, 189, 0.6)', // 薄荷綠色
                'rgba(166, 206, 57, 0.6)'  // 橄欖綠色
            ],
            borderColor: [
                'rgba(41, 52, 80, 0.6)',   // 深藍灰色
                'rgba(74, 144, 226, 0.6)', // 明亮藍色
                'rgba(43, 214, 109, 0.6)', // 柔亮綠色
                'rgba(127, 239, 189, 0.6)', // 薄荷綠色
                'rgba(166, 206, 57, 0.6)'  // 橄欖綠色
            ],
            borderWidth: 1
        }]
    },
    options: {
        scales: {                               //圖表的坐標軸和刻度
            r: {                                //圖的半徑
                beginAtZero: true,              //刻度從零開始 
                min: 0,                         // 強制從 0 開始
                max: 5,                         // 控制圖表半徑的最大值
                ticks: {                        // 刻度的相關屬性
                    stepSize: 1,                // 每個刻度 1
                    max: 5,                     // 控制坐標軸顯示的最大刻度值
                    font: {
                        size: 16,               // 設定字體大小
                        //weight: 'bold',       // 設定字體粗細
                        lineHeight: 1.5         // 設定行高
                    },
                    color: 'black'
                },
                angleLines: {                   //輻射線的屬性
                    display: true               // 顯示輻射線
                },
                grid: {
                    circular: true              //設置網格為圓形
                }
            }
        },
        plugins: {                                                      //設置圖表的插件選項
            tooltip: {                                                  //提示框的屬性
                callbacks: {                                            //提示框中顯示的內容
                    label: function (context) {                         //label 函數，會在顯示提示框時被調用
                        return `${context.label}: ${context.raw}`;      // 當前數據點的標籤跟原始數值
                    }
                }
            },
            legend: {
                labels: {
                    font: {
                        size: 16   // 調整圖例的文字大小
                    },
                    color: '#333'   // 圖例文字顏色（可選）
                }
            },
        }
    }
});
// 發送 API 請求到後端 抓香氣蒜味等等..
$.ajax({
    url: '/Detail/Taste_Distribution_Api',
    type: 'POST',
    data: { id: shareid },
    success: function (response) {
        //console.log(response.fragrance);
        //console.log(response.sour);
        //console.log(response.bitter);
        //console.log(response.sweet);
        //console.log(response.strong);
        polarAreaChart.data.datasets[0].data = [
            response.fragrance,
            response.sour,
            response.bitter,
            response.sweet,
            response.strong
        ];

        // 更新圖表
        polarAreaChart.update();
    },
    error: function (xhr, status, error) {
        console.log('錯誤:', xhr.responseText);
    }
});





////////////////////////////////////////////// 咖啡沖泡計算機-參數設定區 //////////////////////////////////////////////

function Select_Choose() {
    if ($('.cooffee-drink-select').val() === "other") {
        $('.coffee-drink-input').val('');
        $('.coffee-drink-input').prop('disabled', false);
        $('.coffee-drink-input').prop('placeholder', 'Enter the integer value.');


    } else {
        $('.coffee-drink-input').val($('.cooffee-drink-select').val());
        $('.coffee-drink-input').prop('disabled', true)
    }
}
// 設置數據的單位 
var chartData = [
    { unit: "ml" },
    { unit: "g" },
    { unit: "min" },
    { unit: "°C" },
    { unit: "ml" },
    { unit: "sec" }
];

// 設置上下限拿來計算 圖表的series (百分比)
var limits = [
    { min: 0, max: 1000 },    // 水量
    { min: 0, max: 66 },     // 咖啡粉
    { min: 90, max: 180 },   // 沖煮時間（秒）
    { min: 90, max: 96 },    // 水溫
    { min: 0, max: 132 },     // 悶蒸水量
    { min: 20, max: 60 }     // 悶蒸時間（秒）
];

var elements = document.querySelectorAll('.gaugeChart');
var charts = {}; // 儲存每個圖表物件，方便後續更新


// 初始化圖表 讓他一開始就呈現在畫面(不顯示數據)
elements.forEach(function (element, index) {
    var data = chartData[index];

    var options = {
        chart: {
            height: 180,
            type: "radialBar",
            offsetY: 20
        },
        series: [100],  // 初始時設為100
        colors: ["#20E647"],
        plotOptions: {
            radialBar: {
                hollow: {
                    margin: 0,
                    size: "66%",
                    background: "#293450"
                },
                track: {
                    dropShadow: {
                        enabled: true,
                        top: 2,
                        left: 0,
                        blur: 4,
                        opacity: 0.15
                    }
                },
                dataLabels: {
                    name: {
                        show: false  // 不顯示標籤名稱
                    },
                    value: {
                        show: false  // 不顯示數值
                    }
                }
            }
        },
        fill: {
            type: "gradient",
            gradient: {
                shade: "dark",
                type: "vertical",
                gradientToColors: ["#87D4F9"],
                stops: [0, 100]
            }
        },
        stroke: {
            lineCap: "round"
        },

    };

    var chart = new ApexCharts(element, options);
    chart.render();
    // 儲存圖表物件 方便後續更新
    charts["chart" + index] = chart;
});

/////////////////////////////////////咖啡沖泡計算機-開始計算按鈕 畫面下移/////////////////////////////////////////
$(".btn-calculate").on('click', function () {

    var input_value = $('.coffee-drink-input').val();
    if (input_value < 50 || input_value > 1000 || isNaN(input_value)) {
        // 輸入不符合條件，跳出警示框
        alert('Error：Enter the integer value between 50 and 1000.\nExcessive brewing will affect the optimal flavor!');
        $('.coffee-drink-input').val(''); // 清空輸入框
        $('.coffee-drink-input').prop('placeholder', 'Please enter valid integer again.');
        elements.forEach(function (element, index) {
            var chart = charts["chart" + index];
            chart.updateOptions({
                series: [100], // 保持圖表數據為初始值100
                plotOptions: {
                    radialBar: {
                        dataLabels: {
                            name: {
                                show: false // 不顯示標籤名稱
                            },
                            value: {
                                show: false // 不顯示數值
                            }
                        }
                    }
                }
            });
        });
    } else {
        // 輸入符合條件
        document.getElementById("target").scrollIntoView({
            behavior: "smooth"
        });
    }
});
///////////////////////////////////////////咖啡沖泡計算機-計算結果區/////////////////////////////////////////////
// 取得 烘焙程度 後續可尼拿來判斷 研磨粗細、萃取係數、注水方式 ， 沖煮時間跟水溫也會用到
var baking_level = $('.baking-level-answer').text();
console.log(baking_level);

function calculateParameters() {

    const baking_data = [
        { "baking": "Light", "watertemperature": "90-92", "brewtime": "2:30-3:00" },
        { "baking": "Cinnamon", "watertemperature": "91-93", "brewtime": "2:15-2:45" },
        { "baking": "Medium", "watertemperature": "92-94", "brewtime": "2:00-2:30" },
        { "baking": "City", "watertemperature": "93-95", "brewtime": "1:45-2:15" },
        { "baking": "French", "watertemperature": "94-96", "brewtime": "1:30-2:00" },
    ];
    // 使用者輸入的咖啡飲用量
    const coffee_drink_input = $('.coffee-drink-input').val();
    // 粉水比 正常 1:15
    var coffee_water_ratio;
    switch ($('.strong-light-preference').val()) {
        case '1':
            coffee_water_ratio = 14;
            break;
        case '2':
            coffee_water_ratio = 15;
            break;
        case '3':
            coffee_water_ratio = 16;
            break;
        default:
            coffee_water_ratio = 9999999;
    }
    //console.log($('.strong-light-preference').val());
    // 水量
    var water = coffee_drink_input;
    // 咖啡粉重量
    var coffee_weight = (coffee_drink_input / coffee_water_ratio).toFixed(1);
    if (coffee_weight.endsWith('.0')) {

        coffee_weight = parseInt(coffee_weight, 10).toString();
    } else {
        coffee_weight = coffee_weight;
    }
    // 悶蒸水量
    var simmer_water = (coffee_weight * 2).toFixed(0);
    // 悶蒸時間
    var simmer_time;
    switch ($('.taste-preference').val()) {
        case '1':
            simmer_time = "25-35";
            break;
        case '2':
            simmer_time = "30-40";
            break;
        case '3':
            simmer_time = "35-45";
            break;
        default:
            simmer_time = "99999999-9999999999999";
    }
    console.log($('.taste-preference').val());
    // 沖煮時間  先用class="thisanswer"等之後資料庫連線在看用什麼方法抓到
    var brew_time = baking_data.find(s => s.baking === baking_level).brewtime;

    // 水溫
    var water_temperature = baking_data.find(s => s.baking === baking_level).watertemperature;
    // 每個計算結果的陣列
    const calculate_data = [water, coffee_weight, brew_time, water_temperature, simmer_water, simmer_time]

    // 更新 數據 & 圖表
    chartData.forEach(function (data, index) {
        const limit = limits[index];
        const value = calculate_data[index];
        /*console.log(value);*/
        let min_value;
        if (data.unit === "min") {
            const [star, end] = value.split('-');
            const [m1, s1] = star.split(':').map(Number);
            const [m2, s2] = end.split(':').map(Number);
            min_value = ((m1 + m2) * 60 + s1 + s2) / 2;
        } else if (data.unit === "°C" || data.unit === "sec") {
            const [star, end] = value.split('-').map(Number);
            min_value = (star + end) / 2;
        } else {
            min_value = Number(value);
        }
        const percentage = ((min_value - limit.min) / (limit.max - limit.min)) * 100;

        // 更新圖表
        charts["chart" + index].updateOptions({
            series: [percentage],
            plotOptions: {
                radialBar: {
                    dataLabels: {
                        value: {
                            color: "#fff",
                            fontSize: "20px",
                            show: true,
                            offsetY: -20,
                            formatter: function () {
                                return value;
                            }
                        },
                        name: {
                            color: "#fff",
                            fontSize: "15px",
                            show: true,
                            offsetY: 20,
                            formatter: function () {
                                return data.unit;
                            }
                        }
                    }
                }
            }
        });
    });

}
// 發送 API 請求到後端 GET改成用POST 取得對應的 研磨粗細、萃取係數、注水方式
$.ajax({
    url: '/DetailEn/Grinding_Thickness_Extraction_Coefficient_Wate_Injection_Method_Api_En',
    type: 'POST',
    data: { baking_level: baking_level },
    success: function (response) {
        //console.log(response.grindsize);
        // 更新研磨粗細顯示結果
        $('.grind-size-result').text(response.grindsize);
        $('.extraction-conditions-result').text(response.extractionconditions);
        $('.add-water-result').text(response.addwater);
    },
    error: function () {
        // 顯示錯誤訊息
        $('#grindSizeResult').text('Error');
    }
});

// test - 2 ---------------------------------------------------------------------------------------//

let chatbotData;

// 加載 JSON 數據
$.ajax({
    url: '/data/chatbotEn.json',
    method: 'GET',
    dataType: 'json',
    cache: false, // 防止快取
    success: function (data) {
        //console.log("加載的數據：", data);
        chatbotData = data;
        initializeChatbotOptions(chatbotData);
    },
    error: function () {
        $('#chatbot-body').append('<div class="message-bot">The data could not be loaded. Please retry later.</div>');
    }
});
scrollToBottom();

// 聊天室開關
$('#chatbot-toggle').on('click',function () {
    $('#chatbot-window').toggleClass('d-none');
});

$('#chatbot-close').on('click', function () {
    $('#chatbot-window').addClass('d-none');
});

// 初始化聊天選項
function initializeChatbotOptions(data) {
    const options = Object.keys(data).map(category => `
            <button class="chat-option" data-category="${category}">
                ${category}
            </button>
        `).join('');
    $('#chatbot-body').append(`
            <div class="message-bot">
                Please choose an option：
                <div class="chat-options">
                    ${options}
                </div>
            </div>
        `);

    // 綁定選項按鈕點擊事件
    $('#chatbot-body').off('click').on('click', '.chat-option', function () {
        const category = $(this).data('category');
        handleCategory(chatbotData[category], category);
    });
}

// 處理按鈕點擊邏輯
function handleCategory(data, category) {
    // 顯示用戶消息
    $('#chatbot-body').append(`
            <div class="message-user">
                ${category}
            </div>
        `);
    scrollToBottom();


    if (Array.isArray(data) && data[0]?.name) {
        // 處理團隊介紹卡片
        const cards = data.map(member => `
        <div class="col-md-6 mb-4">
            <div class="card bg-dark text-white">
                <img src="${member.image}" class="card-img" alt="${member.name}" style="height: 300px; object-fit: cover;">
                <div class="card-img-overlay">
                    <h5 class="card-title">${member.name}</h5>
                    <p class="card-text"><strong>${member.position}</strong></p>
                    <p class="card-text">${member.description}</p>
                </div>
            </div>
        </div>
    `).join('');

        setTimeout(() => {
            $('#chatbot-body').append(`
            <div class="message-bot">
                <div class="row">
                    ${cards}
                </div>
            </div>
        `);
            scrollToBottom();
        }, 400);
    } else if (Array.isArray(data)) {

        if (category === "Branding Timeline") {
            generateTimeline(data);
        } else {
            // 針對 咖啡冷知識陣列 隨機選取一則文案
            const randomIndex = Math.floor(Math.random() * data.length);
            const randomFact = data[randomIndex];

            // 顯示機器人回應
            setTimeout(() => {
                $('#chatbot-body').append(`
                    <div class="message-bot">
                        ${randomFact}
                    </div>
                `);
                scrollToBottom();
            }, 400); // 模擬延遲
        }
    } else if (typeof data === 'string') {
        // 單一文本的邏輯
        setTimeout(() => {
            $('#chatbot-body').append(`
                    <div class="message-bot">
                        ${data}
                    </div>
                `);
            scrollToBottom();
        }, 400); // 模擬延遲
    } else {
        // 子選項的邏輯
        const options = Object.keys(data).map(option => `
                <button class="chat-option" data-category="${option}">
                    ${option}
                </button>
            `).join('');

        setTimeout(() => {
            $('#chatbot-body').append(`
                    <div class="message-bot">
                        Please choose an option：
                        <div class="chat-options">
                            ${options}
                        </div>
                    </div>
                `);
            scrollToBottom();

            // 綁定新選項按鈕的點擊事件
            $('#chatbot-body').off('click').on('click', '.chat-option', function () {
                const subCategory = $(this).data('category');
                handleCategory(data[subCategory], subCategory);
            });
        }, 400); // 模擬延遲
    }
}

// 滾動到底部
function scrollToBottom() {
    const chatBody = $('#chatbot-body');
    chatBody.scrollTop(chatBody[0].scrollHeight);
}
// 動態生成時間軸的功能
function generateTimeline(timelineData) {
    const timelineEntries = timelineData.map(item => `
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-content">
                <h5 class="timeline-year">${item.year}</h5>
                <h6 class="timeline-title">${item.title}</h6>
                <p class="timeline-description">${item.description}</p>
            </div>
        </div>
    `).join('');

    setTimeout(() => {
        $('#chatbot-body').append(`
            <div class="message-bot">
                <div class="timeline">
                    ${timelineEntries}
                </div>
            </div>
        `);
        scrollToBottom();
    }, 500);
}   



