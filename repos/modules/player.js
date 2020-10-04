export var play = {};
(function() {

    this.sayHi = function (name)
    {
        return 'hi ' + name + '!';
    }

    function sayThat(name)
    {
        return 'hi ' + name + '!';
    }

}).apply(play);
