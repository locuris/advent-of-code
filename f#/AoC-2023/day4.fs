module day4

let split(text: string) : string array =
    text.Split('|')

type ScratchCard =
    struct
        val Id: int
        val WinningNumbers: int array
        val CardNumbers: int array
        val Matches: int 
        
        new(input: string) = {
            Id = (input.Split(':')[0]).Split(' ') |> Array.last |> int
            WinningNumbers = ((input.Split(':')[1]).Split('|')[0]).Split(' ') |> Array.filter (fun s -> not (s = "")) |> Array.map int
            CardNumbers = ((input.Split(':')[1]).Split('|')[1]).Split(' ') |> Array.filter (fun s -> not (s = "")) |> Array.map int
            Matches = ((input.Split(':')[1]).Split('|')[1]).Split(' ') |> Array.filter (fun s -> not (s = "")) |> Array.map int |> Array.sumBy (fun n -> if Array.contains n (((input.Split(':')[1]).Split('|')[0]).Split(' ') |> Array.filter (fun s -> not (s = "")) |> Array.map int) then 1 else 0) 
        }        
        
        member this.Winnings(winningNumbers: int array) =            
            let n = this.CardNumbers |> Array.sumBy (fun n -> if Array.contains n winningNumbers then 1 else 0)
            if n > 1 then pown 2 (n - 1)  else n
    end
    
let part1(lines: string array) =
    lines |> Array.map ScratchCard |> Array.sumBy (fun card -> card.Winnings card.WinningNumbers) |> string
    
let part2(lines: string array) =
    let cards = lines |> Array.map ScratchCard    
    let mutable copies = cards |> Array.map (fun card -> card, 0) |> Map.ofArray 
    cards |> Array.iteri (fun i card ->
        copies <- copies.Add(card, copies[card] + 1)
        for n in 1..card.Matches do
            if i + n < cards.Length then
                copies <- copies.Add(cards[i + n], copies[cards[i + n]] + (1 * copies[card]))
    )
    let mutable answer = 0
    copies |> Map.iter (fun _ v -> answer <- answer + v)
    answer |> string