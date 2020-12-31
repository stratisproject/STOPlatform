///site.js file is for global configuration.

(function ($) {
    M.AutoInit();
    $.validator.setDefaults({ ignore: "[data-val!=true], :hidden:not([data-val=true])" });
})(jQuery);