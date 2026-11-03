window.initOrgChoices = function (dotNetRef) {

    var observers = {};

    function destroyChoices(elementId) {
        var el = document.getElementById(elementId);
        if (!el) return;
        if (el._choicesInstance) {
            try { el._choicesInstance.destroy(); } catch (e) { }
            el._choicesInstance = null;
        }
        if (observers[elementId]) {
            observers[elementId].disconnect();
            delete observers[elementId];
        }
    }

    function setupChoices(elementId) {
        var el = document.getElementById(elementId);
        if (!el) return;

        destroyChoices(elementId);

        var choices = new Choices(el, {
            removeItemButton: true,
            searchEnabled: true,
            searchPlaceholderValue: 'Tìm công ty...',
            noResultsText: 'Không tìm thấy',
            noChoicesText: 'Không có lựa chọn',
            itemSelectText: '',
            placeholder: true,
            placeholderValue: '-- Tất cả tổ chức --',
            maxItemCount: 5,
            maxItemText: function (maxItemCount) {
                return 'Chỉ được chọn tối đa ' + maxItemCount + ' công ty';
            },
            shouldSort: false,
        });

        el._choicesInstance = choices;

        el.addEventListener('change', function () {
            var selected = Array.from(el.selectedOptions).map(function (o) {
                return o.value;
            });
            dotNetRef.invokeMethodAsync('OnOrgSelectionChanged', selected);
        });

        // Theo dõi nếu Blazor xóa/thay thế element — destroy trước khi bị crash
        var parent = el.closest('.choices') ? el.closest('.choices').parentNode : el.parentNode;
        if (parent) {
            var observer = new MutationObserver(function (mutations) {
                mutations.forEach(function (mutation) {
                    mutation.removedNodes.forEach(function (node) {
                        if (node.contains && node.contains(el)) {
                            destroyChoices(elementId);
                        }
                    });
                });
            });
            observer.observe(parent, { childList: true, subtree: true });
            observers[elementId] = observer;
        }
    }

    setTimeout(function () {
        setupChoices('orgMultiSelect');
        setupChoices('orgMultiSelectMobile');
    }, 150);
};

window.destroyOrgChoices = function () {
    ['orgMultiSelect', 'orgMultiSelectMobile'].forEach(function (id) {
        var el = document.getElementById(id);
        if (el && el._choicesInstance) {
            try { el._choicesInstance.destroy(); } catch (e) { }
            el._choicesInstance = null;
        }
    });
};
