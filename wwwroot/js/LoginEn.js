// ------------------------------------------------------- 登入按鈕 ------------------------------------------------------- //

$('.LoginBtn').on('click', function () {
    console.log(12648)

    const UserId = $('#InputUserId').val();
    const Password = $('#InputPassword').val();

    // 傳送資料到後端
    $.ajax({
        url: '/AccountEn/Login',
        type: 'POST',
        data: {
            userId: UserId,
            password: Password
        },
        success: function (data) {
            console.log(data)
            if (data === "0") {
                alert(`Welcome! Let's go shopping!`);
                window.location.href = '/Home/IndexEn';  
            } else if (data === "1") {
<<<<<<< HEAD
                alert('Invalid password. Please try again.');           
            } else if (data === "2") {
                alert('You need to create an account to continue.');
                window.location.href = '/AccountEn/Register';
            } 
=======
                alert('Invalid password. Please try again.');
                window.location.href = '/AccountEn/Register';  
            } else if (data === "2") {
                alert('You need to create an account to continue.');
            }
>>>>>>> 1b05320a2ef143c6f9057d83050129a8bfc832dd
        },
        error: function () {
            alert('Error');
        }
    });
});


// ------------------------------------------------------- 註冊按鈕 ------------------------------------------------------- //
$('.RegisterBtn').on('click',function () {
    window.location.href = "/AccountEn/Register";
})