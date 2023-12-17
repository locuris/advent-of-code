module day7

open System
open Common



let cardToRank = Map [
    ('T', 10)
    ('J', 1)
    ('Q', 12)
    ('K', 13)
    ('A', 14)
]

let getCardRank (card: char) : int =
    if Char.IsDigit card then int(card.ToString()) else cardToRank[card]
    
let getCardScore (cards: Map<char, int>) : int =
    if cards.Values.Contains 5 then
        7
        else if cards.Values.Contains 4 then
            6            
            else if cards.Values.Contains 3 && cards.Values.Contains 2 then
                5
                else if cards.Values.Contains 3 then
                    4
                    else if (Data.countOf 2 (cards.Values |> Array.ofSeq)) = 2  then
                        3
                        else if cards.Values.Contains 2 then
                            2
                            else 1
    
let getSecondaryScore (cards: string) : int array =
    cards.ToCharArray() |> Array.map getCardRank
    
let getCards(hand: string) : Map<char, int> =
    hand.ToCharArray() |> Array.distinct |> Array.map (fun card -> (card, hand.ToCharArray() |> Data.countOf card)) |> Map.ofArray
    
let getCardsWithJokers(hand: string) : Map<char, int> =
    let cards = getCards hand
    if cards.ContainsKey('J') && cards['J'] <> 5 then
        let best = cards |> Map.toArray |> Array.filter (fun (card, _) -> not (card = 'J')) |> Array.maxBy snd |> fst
        cards |> Map.map (fun card count -> if card = best then count + cards['J'] else count) |> Map.filter (fun c _ -> not (c = 'J'))
        else
            cards
    
let getCardsAndBet (getCardsLogic: string -> Map<char, int>) (hand: string) : Map<char, int> * int * int array * string =
    getCardsLogic (hand.Split(' ')[0]), int(hand.Split(' ')[1]), getSecondaryScore (hand.Split(' ')[0]), hand.Split(' ')[0]
    
    
let getAnswer (lines: string array) (getCardsLogic: string -> Map<char, int>) : string = 
    let games = lines |> Array.map (getCardsAndBet getCardsLogic) |> Array.map (fun (cards, bet, score, hand) -> (cards, bet, getCardScore cards, score, hand)) |> Array.toList |> List.sortBy (fun (_, _, rank, score, _) ->
        let a, b, c, d, e = score[0], score[1], score[2], score[3], score[4]
        (rank, a, b, c, d, e)) 
    games |> List.mapi (fun i (_, bet, _, _, _) -> bet * (i + 1)) |> List.sum |> string
    
let part1 (lines: string array) : string =
    getAnswer lines getCards
    
    
let part2 (lines: string array) : string =
    getAnswer lines getCardsWithJokers