document.addEventListener('DOMContentLoaded', function () {
        var forgotPasswordLink = document.getElementById('forgotPasswordLink');
    var usernameInput = document.getElementById('usernameInput');

    forgotPasswordLink.addEventListener('click', function(event) {
            var username = usernameInput.value.trim();
    if(username === '') {
        event.preventDefault(); // Linkin takip edilmesini önle
    alert('Lütfen kullanıcı adınızı giriniz.');
            } else {
        this.href = '/Home/ForgotPassword?username=' + encodeURIComponent(username);
            }
        });
});

function loadMessages(receiverId) {
    // AJAX çağrısı ile server'dan mesajları al
    $.ajax({
        url: '/api/messages/find',
        type: 'GET',
        data: { receiverId: receiverId },
        success: function (data) {

        },
        error: function (error) {
            console.error("Mesajlar yüklenirken bir hata oluştu", error);
        }
    });
}
$(".person").click(function () {
    var receiverId = $(this).find('input[name="receiverId"]').val();
    loadMessages(receiverId);
});