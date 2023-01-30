
open System.IO
open System.Collections.Generic


let readFile path = 
    try
        File.ReadAllBytes path |> Ok
    with
    | exn -> Error (exn.Message)


let createByteHistogram (data: byte[]) =
    let histMap = new Dictionary<byte, uint64>()

    let countItem datum =
        let (found, count) = histMap.TryGetValue(datum)
        if found then histMap.[datum] <- count + 1uL else histMap.[datum] <- 1uL

    Array.iter countItem data
    histMap


let printRawHistogram (histogram: Dictionary<'a, uint64>) =
    for pair in histogram do
        printfn $"{pair.Key}: {pair.Value}"  

[<EntryPoint>]
let main args =
    try
        let path = args[0]
        let dataResult = readFile path
        match dataResult with
        | Ok data ->
            data |> createByteHistogram  |> printRawHistogram
            0
        | Error msg ->
            printfn "Error: %s" msg
            -2
    with
    | exn ->
        printfn "Error: %s" exn.Message
        -1
