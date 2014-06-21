; (function ($) {

    var pluginName = 'trForm',

        defaults = {
            validateBeforeSubmit: true,
            submitBtn: 'submit, .submit',
            notifyTimeout: 3000,
            notifyMessageContainer: '.notify > .text'
            // http-status-code: callback
            // success: callback
            // error: callback
        };

    function trForm(element, options) {
        this.element = element;
        this.options = $.extend({}, defaults, options);
        this.init();
    }

    trForm.prototype = {
        _beforeSubmit: function (arr, $form) {

            

            var base = $.data($form[0], pluginName);

            if ($(base.options.submitBtn, base.element).hasClass('disabled')) return;

            var isValid = true; var firstInvalidInput;
            $('input[required]:visible, textarea[required]:visible', $form).each(function () {
                
                isValid = isValid && $(this).val() != '';
                
                if (!isValid && firstInvalidInput == null) {
                    firstInvalidInput = this;
                }
                //$(this).val() == '' ? $(this).addClass('invalid') : $(this).removeClass('invalid');

            });
            if (!isValid)
                firstInvalidInput.focus();
            else if (!base.submitted) {

                base.submitted = true;
                $(base.options.submitBtn, $form).stop()
                                                .clearQueue()
                                                .addClass('disabled');

            }

            return isValid;
        },
        _success: function (data, statusText, xhr, $form) {
            var base = $.data($form[0], pluginName);
            base.submitted = false;
            $(base.options.submitBtn, base.element).removeClass('disabled');

            if (base.options[data.status])
                base.options[data.status].call(base, data, statusText, xhr);
            else if (base.options.success)
                base.options.success.call(base, data, statusText, xhr);

            
        },
        _error: function (data, statusText, xhr, $form) {
            var base = $.data($form[0], pluginName);
            base.submitted = false;
            $(base.options.submitBtn, base.element).removeClass('disabled');

            if (base.options[data.status])
                base.options[data.status].call(base, data, statusText, xhr);
            else if(base.options.error)
                base.options.error.call(base, data, statusText, xhr);

            

        },
        notify: function (cssClass, message, notifyMessageContainer) {
            var lnk = $(this.options.submitBtn, this.element);

            $((notifyMessageContainer || this.options.notifyMessageContainer), lnk).html(message);

            lnk.removeClass('disabled')
                .addClass(cssClass)
                .stop()
                .clearQueue()
                .delay(this.options.notifyTimeout).queue(function () {
                    lnk.removeClass(cssClass);
                });
        },
        clear: function() {

            this.element.find('input:not([type="button"]), textarea').val('');

        },
        submit: function (e) {
            if (e && e.keyCode && e.keyCode != 13)
                return;

            var base = (this.nodeName.toLowerCase() == 'form' ? $(this) : $(this).closest('form')).data(pluginName);

            base.element.submit();

            if (e && e.keyCode && e.keyCode == 13) {
                e.preventDefault();
                return false;
            }
        },
        init: function () {
            this.element.ajaxForm({
                beforeSubmit: this._beforeSubmit,
                success: this._success,
                error: this._error
            }).keydown(this.submit)
              .find(this.options.submitBtn).off('click', this.submit).click(this.submit);
        },
        destroy: function () {
            this.element.off('keydown', this.submit)
                        .resetForm()
                        .removeData(pluginName)
                        .find(this.options.submitBtn).off('click', this.submit);
        }
    };

    $.fn[pluginName] = function (options) {

        var args = arguments;
        options = options || {};

        return this.each(function () {

            var cached = $.data(this, pluginName);
            if (cached) {
                if (options.substring)
                    cached[options].apply(cached, [].splice.call(args, 1));
                return true;
            }
            else if (options.substring)
                throw new Error(pluginName + ' not available for this DOM element!');
            cached = $(this);

            cached.data(pluginName, new trForm(cached, options));

            return true;

        });
    };

})(jQuery);