using Kirisoup.Diagnostics.TypeUsageRules;

_ = new Bar();
_ = (Bar)new();
_ = default(Bar);
_ = (Bar)default;

_ = new Foo();
_ = (Foo)new();
_ = default(Foo);
_ = (Foo)default;

_ = new Foo(0);
_ = (Foo)new(0);

[NoDefault]
[NoNew]
readonly struct Foo(byte _);

readonly struct Bar;