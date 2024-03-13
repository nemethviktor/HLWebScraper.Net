
# Welcome to the (totally unofficial) Hargeaves Lansdown Web Scraper/Parser

Most of all please note that this product is neither affiliated with nor endorsed by HL. That's also to say I didn't include their logo in the About box as I presume that's a trademarked thing. 
The usual caveats apply: I take no responsibility for whatever outcome you encounter through using this app and none of what's presented in the app is financial advice.

## Software Description

The aim of this app is to collate all (viable) securities (not funds) from the HL website and allow for some form of personal (user-based) classification. At the end of the process the data can be saved into a CSV for further processing.
Things this collects (each subject to availability):
- URL 
- Name 
- Whether a security can be held in an ISA 
- Ticker 
- SEDOL
- Sector (kinda-custom)
- ETF_Type (user-defined, see below)
- Exchange 
- Country 
- Indices 
- Currency 
- Open_price 
- Year_low 
- Year_high 
- Dividend_yield 
- PE_ratio 
- Market_capitalisation 
- Volume 
- GBP_Open 
- GBP_Year_low 
- GBP_Year_high 
- GBP_Market_capitalisation 

As mentioned above the app doesn't pull Funds. The main reason behind this is that there's an API for Funds and so you can use Postman for that but there's no public API for stocks and whatnots so it's very difficult to analyse what to invest in.
On that note the app can only collect stuff that's either visibly available on the site or is at least in the source code of the HTML data - that is to say I don't mind entertaining feature requests but I can only pull what's "there". Also note some of the results that are returned can be blanks or "n/a". Again that's just what the website contains.

## System Requirements
- Requires Windows 10. Lower versions kept throwing SSL errors so I disabled Windows 7/8.x, sorry. I think they're pretty uncommon by this point anyway.
- Requires .NET Windows Desktop Runtime v8 - Windows should trigger a manual install request if missing, it's a file download straight from Microsoft's website.

## Download & Install (Windows 10+ only)

There's no installation file per se. Unzip and run the exe.

## Things to Note, Usage

### Usage

- Usage is simple - the main aim of the app is to extract, categorise and dump data into a CSV so all you need to do is press the Start button and then once done Save
- The Overview tab becomes active when the Scrape process has completed. It's not really meant to be a proper place for analysis but just a generic overview opportunity. Excel/your favourite CSV feeder can do a better job.
- I didn't include any particular code-smartness for corporate proxy handling, not the least because while I wholeheartedly don't care/mind if you use this at work but I don't/can't so I couldn't test any of that stuff while developing. If you want, submit a PR and I can include it.

### ETF Classifications

- There's a CSV file called ETF_Types_example.csv in the Resources folder. It works on a "contains" [_not_ case sensitive!] basis and each line is taken into consideration in order or precedence. When a "contains" match is found the loop exits. 
  - If you want to modify that file feel free to but the way the code works is that:
    - it checks if there's a ETF_Types.csv
    - if so then it will take that and ignore ETF_Types_example.csv
    - otherwise it will take ETF_Types_example.csv
    - remember that if this repo ever gets updated then ETF_Types_example.csv may or may not be overwritten therefore I strongly suggest you copy to and modify ETF_Types.csv to your own liking
    - you can use the relevant button in the app to reload the ETF_Types.csv and then output the updated results into a new file (you _do_ need to press the Save Data button for that to happen.)
    - it is expected that ETF_Types_example.csv//ETF_Types.csv have the column structure as in the example file, otherwise the contents will be ignored and/or the code might break.

### Things to Note

- The whole end-to-end process takes a fair bit of time so be patient.
- Avast antivirus is a total f...ing idiot and flags the app as suspicious. While I'd love to point out that the source code is public and everyone can check it the TLDR is that there's nothing suspcious about the app.
- The CancellationToken (aka Stop Button) logic is a bit patchy I think. Might be imperfect.
- In case you build upon the output of this app (ie you feed the CSV into whatever) I'd suggest you base your ETL on column names and not column order as I don't promise that latter will never change.
- Some securities are discarded entirely, such as when the Ticker or Name or SEDOL looks invalid or is missing or they can't otherwise be traded due to the lack of KID.
- For those better versed in C# this code has been ported from VBA, which has two main implications:
    - I didn't really bother bringing in external libraries for parsing or making the code particularly neat. I had a look at HTMLAgility but in reality fishing around XPath is a pain in the backside when I already have my somewhat loop-ing code from VBA cast to C# by GPT.
    - The version number is seemingly odd: we start with v4 because the previous ones were my own VBA stuff
- There's some semblance of a Dark Mode option. WinForms doesn't actually support DM natively and can't replicate it like-for-like so it's as good as it gets for now.
- The app uses a library (`CompressedMemoryCache.cs`) - the licence of that is contained in the file and was built by Gustavo Augusto Hennig (it's APACHE 2.0 btw)
    - It's necessary to use compression on the html pages because storing that many (read: tens of thousands) pages at 200-400kbytes each will eat up memory in no time. My initial tests of letters A-C made the app consume around 5GB RAM w/o compression and sub-1GB w/ compression.

## Pull Requests

I'm generally happy for anyone competent to add pull requests but I don't always sync the commits as they come with GitHub until there is an actual release - it's therefore possible that my local commits are lightyears away from the public ones. If you'd like to do a pull request, drop a message/open a ticket first please and we can sync the details.

## Known Issues

- "Ticker" extraction works on the basis of getting whatever's in the last parentheses of the header. This is because HL website doesn't appear to store this info separately. In some rare cases (espc w/ Trusts) this can yield odd results. (e.g. [here](https://www.hl.co.uk/shares/shares-search-results/b/baillie-gifford-us-growth-trust-ord) we'd pick up "USA", which is wrong.)
- The "Sector" classification can occasionally mislabel securities as ETFs particularly when their name contains the word "Fund" or "Income". HL doesn't actually have an ETF-flag so the code attempts to decipher what's what. This works around 98% of the cases.

## When reporting bugs please specify
- The OS's version,
- The OS's language (incl region such as English/UK or English/USA),
- The particular URL you're having issues with if applicable.