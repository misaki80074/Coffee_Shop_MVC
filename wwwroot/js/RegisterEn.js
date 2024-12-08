// ------------------------------------------------------- 全部input 限制不能輸入空格 ------------------------------------------------------- //
$('input').on('keypress', function (e) {
    if (e.which === 32) {        //  ASCII 編碼
        e.preventDefault();      // 阻止輸入空格
        alert('Spaces are prohibited.');
    }
});

// ------------------------------------------------------- 帳號判斷 有無註冊過 ------------------------------------------------------- //
// 先記錄上次輸入的帳號值
let FrontInputUserId = ''; 
$('#InputUserId').on('blur', function () {
    const UserId = $(this).val();

    // 輸入有空格 跳出不發送AJAX
    if (!UserId.trim()) {
        $('#InputUserIdResult').text('Please input your account.').css('color', 'orange');
        return;
    }

    // 如果跟上次輸入相同 跳出不發送AJAX
    if (UserId === FrontInputUserId) {
        return;
    }

    // 更新上次的輸入值
    FrontInputUserId = UserId; 
    
    $.ajax({
        url: '/AccountEn/CheckUserid',
        type: 'POST',
        //contentType: 'application/json',
        // 帳號轉成 JSON 格式
        data: JSON.stringify(UserId), 
        success: function (data) {
            // 0 帳號存在 | 1帳號尚未註冊
            if (data === "0") {
                $('#InputUserIdResult').text('This account is already in use.').css('color', 'red');
            } else if (data === "1") {
                $('#InputUserIdResult').text('This account can be used.').css('color', 'green');
            }
        },
        error: function () {
            $('#InputUserIdResult').text('Error').css('color', 'red');
        }
    });
});

// ------------------------------------------------------- 密碼判斷 大小寫+數字+至少8碼 ------------------------------------------------------- //
$('#InputPassword').on('input', function () {
    const PassWord = $(this).val();  

    // 檢查密碼是否符合條件
    const PassWordLength = 8;

    // 是否包含大寫字母
    const HasUpperABC = /[A-Z]/.test(PassWord);  

    // 是否包含小寫字母
    const HasLowerABC = /[a-z]/.test(PassWord);  

    // 是否包含數字
    const HasNumber = /[0-9]/.test(PassWord);     

    // 顯示密碼強度
    if (PassWord.length < PassWordLength) {
        $('#InputPasswordResult').text('The password must be at least 8 characters.').css('color', 'red');
    } else if (!HasUpperABC || !HasLowerABC || !HasNumber) {
        $('#InputPasswordResult').text('Your password must contain at least one uppercase letter, one lowercase letter, and one number.').css('color', 'red');
    } else {
        $('#InputPasswordResult').text('Valid.').css('color', 'green');

    }
});

// ------------------------------------------------------- 再次輸入密碼確認 ------------------------------------------------------- //
$('#CheckPassword').on('input', function () {
    const PassWord = $('#InputPassword').val();
    const CheckPassword = $(this).val();

    // 檢查兩個密碼欄位是否一致
    if (CheckPassword !== PassWord) {
        $('#CheckPasswordResult').text('Not match').css('color', 'red');
    } else {
        $('#CheckPasswordResult').text('Match').css('color', 'green');
    }
});

// ------------------------------------------------------- 手機格式判斷 ------------------------------------------------------- //
$('#InputPhone').on('input', function () {
    const InputPhone = $(this).val();  
    const PhonePattern = /^09\d{8}$/;

    // 檢查手機號碼是否符合格式
    if (!PhonePattern.test(InputPhone)) {
        $('#InputPhoneResult').text('Invalid format').css('color', 'red');
    } else {
        $('#InputPhoneResult').text('Valid').css('color', 'green');
    }
});

// ------------------------------------------------------- 信箱判斷 ------------------------------------------------------- //
$('#InputEmail').on('input', function () {
    const InputEmail = $(this).val();
    const EmailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    // 檢查手機號碼是否符合格式
    if (!EmailPattern.test(InputEmail)) {
        $('#InputEmailResult').text('Invalid format').css('color', 'red');
    } else {
        $('#InputEmailResult').text('Valid').css('color', 'green');
    }
});

// ------------------------------------------------------- 註冊資料提交到後端 ------------------------------------------------------- //
$('#SubmitButton').on('click', function (e) {
    // 防止表單默認提交
    e.preventDefault();  

    // 序列化表單資料  ( 後端要用FROMFROM
    const FormData = $('#registerForm').serialize();  

    $.ajax({
        url: '/AccountEn/Register',
        type: 'POST',
        data: FormData,  // 發送序列化後的表單資料
        success: function (data) {
            // 如果註冊成功，顯示成功訊息並導向首頁
            console.log(data.status)
            if (data.status === "註冊成功") {
                alert('Registration complete.');
                window.location.href = '/Home/IndexEn';  
            } else {
                alert('Registration failed. Please try again later.');
            }
        },
        error: function () {
            alert('Error');
        }
    });
});