// ------------------------------------------------------- 登入按鈕 ------------------------------------------------------- //

$('.LoginBtn').on('click', function () {
    console.log(12648)

    const UserId = $('#InputUserId').val();
    const Password = $('#InputPassword').val();

    // 傳送資料到後端
    $.ajax({
        url: '/Member/Login',
        type: 'POST',
        data: {
            userId: UserId,
            password: Password
        },
        success: function (data) {
            console.log(data)
            if (data === "0") {
                alert('ログイン完了！お買い物をお楽しみください！');
                window.location.href = '/Home/IndexJp';  
            } else if (data === "1") {
                alert('パスワードが一致しません。もう一度お試しください。');
                window.location.href = '/AccountJp/Register';  
            } else if (data === "2") {
                alert('まだ会員登録がお済みではありません。こちらから登録手続きをお願いします。');
            }
        },
        error: function () {
            alert('エラー');
        }
    });
});


// ------------------------------------------------------- 註冊按鈕 ------------------------------------------------------- //
$('.RegisterBtn').on('click',function () {
    window.location.href = "/AccountJp/Register";
})