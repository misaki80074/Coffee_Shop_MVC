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
        $('#chatbot-body').append('<div class="message-bot">Unable to load data. Please try again later.</div>');
    }
});
scrollToBottom();

// 聊天室開關
$('#chatbot-toggle').on('click', function () {
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
                Please make a selection.
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
                <img src="${member.image}" class="card-img" alt="${member.name}" style="height: 200px; object-fit: cover;">
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
        if (isUrl(data)) {
            // 處理 URL 超連結
            const resolvedUrl = resolveUrl(data);
            setTimeout(() => {
                $('#chatbot-body').append(`
                    <div class="message-bot">
                        <a href="${resolvedUrl}" target="_self" class="btn btn-primary bg-success">Click!</a>
                    </div>
                `);
                scrollToBottom();
                
            }, 400);
        } else {
            setTimeout(() => {
                $('#chatbot-body').append(`
                    <div class="message-bot">
                        ${data}
                    </div>
                `);
                scrollToBottom();
                
            }, 400);
        }
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
                        Please make a selection.
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

// 判斷是否為 URL
function isUrl(string) {
    const urlRegex = /^(https?:\/\/|~\/)/i;
    return urlRegex.test(string);
}

// 解決 URL，處理 ~/ 前綴
function resolveUrl(url) {
    if (url.startsWith('~/')) {
        return window.location.origin + url.slice(1);
    }
    return url;
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

