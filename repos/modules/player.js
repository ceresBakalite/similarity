export { play }

var play = {};
(function() {

    let test = null;

    this.get = function (name)
    {
        return 'hi ' + name + '!';
    }

    this.set = function (name)
    {
        test = 'hi ' + name + '!';
    }

}).apply(play);

/*
function sayHi(name)
{
    return 'hi ' + name + '!';
}

function sayThat(name)
{
    return 'hi ' + name + '!';
}

export { sayHi, sayThat }
*/
