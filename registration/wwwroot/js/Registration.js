$(document).ready(function () {
    $('#password').keyup(function () {
        var password = $('#password').val();

        if (password === '') {
            clearAll();
            return;
        }

        var restrictedItems = [];
        restrictedItems.push($('#email').val());

        var result = zxcvbn(password, restrictedItems),
            score = result.score,
            message = result.feedback.warning || score < 3 ? 'The password is weak' : '',
            suggestions = result.feedback.suggestions;

        setProgressBar(score);

        $('#submission').prop('disabled', score < 3);
        $('#message').text(message);

        var suggestionHtml = '';
        for (var i = 0; i < suggestions.length; ++i) {
            suggestionHtml += '<p>' + suggestions[i] + '</p>';
        }

        $('#suggestions').html(suggestionHtml);
    });

    function clearAll() {
        $('#strengthBar').attr('class', '').css('width', '0');
        $('#submission').prop('disabled', true);
        $('#message').text('');
        $('#suggestions').html('');
    }

    function setProgressBar(score) {

        var $bar = $('#strengthBar');

        switch (score) {
            case 0:
                $bar.attr('class', 'progress-bar progress-bar-danger')
                    .css('width', '1%');
                break;
            case 1:
                $bar.attr('class', 'progress-bar progress-bar-danger')
                    .css('width', '25%');
                break;
            case 2:
                $bar.attr('class', 'progress-bar progress-bar-danger')
                    .css('width', '50%');
                break;
            case 3:
                $bar.attr('class', 'progress-bar progress-bar-warning')
                    .css('width', '75%');
                break;
            case 4:
                $bar.attr('class', 'progress-bar progress-bar-success')
                    .css('width', '100%');
                break;
        };
    }
});