$(document).ready(function () {
    $('.mb-form-widget').each(function () {
        var $form = $(this);
        var src = $form.data('src');
        $form.html('<iframe src="' + src + '" class="mb-form-widget-frame"></iframe>');
    });
});