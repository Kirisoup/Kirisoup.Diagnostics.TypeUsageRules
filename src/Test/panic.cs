using Kirisoup.Diagnostics.PreventDefault;

_ = new Bar();
_ = (Bar)new();
_ = default(Bar);
_ = (Bar)default;

// _ = new Foo();
// _ = (Foo)new();
// _ = default(Foo);
// _ = (Foo)default;

[PreventDefault]
[PreventDefaultCtor]
readonly struct Foo;

readonly struct Bar;