![Icon](http://i.imgur.com/w43BUH9.png?1) 
# Dispenser 
[![Build status](https://ci.appveyor.com/api/projects/status/sp5brdhhnq2f0gyw?svg=true)](https://ci.appveyor.com/project/lvermeulen/dispenser) [![license](https://img.shields.io/github/license/lvermeulen/Dispenser.svg?maxAge=2592000)](https://github.com/lvermeulen/Dispenser/blob/master/LICENSE) [![NuGet](https://img.shields.io/nuget/vpre/Dispenser.svg?maxAge=2592000)](https://www.nuget.org/packages/Dispenser/) [![Coverage Status](https://coveralls.io/repos/github/lvermeulen/Dispenser/badge.svg?branch=master)](https://coveralls.io/github/lvermeulen/Dispenser?branch=master) [![codecov](https://codecov.io/gh/lvermeulen/Dispenser/branch/master/graph/badge.svg)](https://codecov.io/gh/lvermeulen/Dispenser) ![](https://img.shields.io/badge/.net-4.5.2-yellowgreen.svg) ![](https://img.shields.io/badge/netstandard-1.6-yellowgreen.svg)

Dispenser detects inserts, updates and deletes between previous and new versions of items, enabling easy change-based integrations.

## Features:
* Compare new items with previous version of items
* Extensible hashing

## Usage:

* Compare new items with previous version of items:
~~~~
StockItem[] previousStock = 
{
    new StockItem("Lumber 2x2", 7),
    new StockItem("Lumber 2x4", 5),
    new StockItem("Lumber 2x8", 4),
    new StockItem("Lumber 2x10", 2)
};

StockItem[] actualStock =
{
    new StockItem("Lumber 2x2", 3),
    new StockItem("Lumber 2x3", 6),
    new StockItem("Lumber 2x4", 3),
    new StockItem("Lumber 2x6", 3),
    new StockItem("Lumber 2x8", 4)
};

var hasher = new Sha1Hasher();
var results = new Dispenser<StockItem, string>()
	.Dispense(actualStock.Hash(hasher), previousStock.Hash(hasher), x => x.Sku);

Assert.NotNull(results);
Assert.True(results.HasChanges);
Assert.Contains(new StockItem("Lumber 2x3", 6), results.Inserts);
Assert.Contains(new StockItem("Lumber 2x6", 3), results.Inserts);

Assert.Contains(new StockItem("Lumber 2x2", 3), results.Updates);
Assert.Contains(new StockItem("Lumber 2x4", 3), results.Updates);

Assert.Contains(new StockItem("Lumber 2x10", 2), results.Deletes);
~~~~

* Extensible hashing:

SHA256 and SHA1 hashing are provided in **Dispenser.Hasher.Sha256** and **Dispenser.Hasher.Sha1**. To implement your own hashing:
~~~~
public interface IHasher
{
	string Hash(object obj);
}
~~~~

## Thanks
* [Soap Dispenser](https://thenounproject.com/term/soap-dispenser/561492/) icon by [ProSymbols](https://thenounproject.com/prosymbols/) from [The Noun Project](https://thenounproject.com)
