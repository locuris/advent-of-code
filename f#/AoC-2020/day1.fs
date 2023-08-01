module day1

// Take a list of numbers and find the two that sum to 2020 then multiply them together

let day1Part1 (numbers: string array) =
    [ for number in numbers do
          let intNumber: int = int number

          for otherNumber in numbers do
              let otherIntNumber = int otherNumber

              if intNumber + otherIntNumber = 2020 then
                  yield intNumber * otherIntNumber ]
        .Head.ToString()

let day1Part2 (numbers: string array) =
    [ for number in numbers do
          let intNumber = int number

          for otherNumber in numbers do
              let otherIntNumber = int otherNumber

              for otherOtherNumner in numbers do
                  let otherOtherIntNumber = int otherOtherNumner

                  if intNumber + otherIntNumber + otherOtherIntNumber = 2020 then
                      yield intNumber * otherIntNumber * otherOtherIntNumber ]
        .Head.ToString()
