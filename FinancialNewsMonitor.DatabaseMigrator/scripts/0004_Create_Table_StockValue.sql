GO
CREATE TABLE [financial].[StockValue]
(
	[Symbol] NVARCHAR(32) NOT NULL,
    [Date] DATE NOT NULL,
    [Open] DECIMAL(7,4) NOT NULL,
    [High] DECIMAL(7,4) NOT NULL,
    [Low] DECIMAL(7,4) NOT NULL,
    [Close] DECIMAL(7,4) NOT NULL,
    [Volume] INT NOT NULL,
    CONSTRAINT [PK_Symbol_Date] PRIMARY KEY ([Symbol], [Date]),
    CONSTRAINT [FK_StockValue_Symbol] FOREIGN KEY ([Symbol]) REFERENCES [financial].[Symbol]([Symbol])
);

GO
CREATE NONCLUSTERED INDEX IX_StockValue_Symbol ON [financial].[StockValue]([Symbol]);

--TEST DATA
--"2024-12-13": {
--"1. open": "232.2500",
--"2. high": "233.7750",
--"3. low": "230.2600",
--"4. close": "230.8200",
--"5. volume": "2757683"
--},
--"2024-12-12": {
--"1. open": "230.6600",
--"2. high": "233.8900",
--"3. low": "230.3800",
--"4. close": "232.2600",
--"5. volume": "4515741"
--},
--"2024-12-11": {
--"1. open": "232.6900",
-- "2. high": "233.0000",
--"3. low": "229.1300",
--"4. close": "230.1200",
--"5. volume": "3872680"
--},