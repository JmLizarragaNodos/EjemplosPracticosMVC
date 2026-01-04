package main

import (
	"context"
	"io/ioutil"
	"os"
	"strconv"

	"github.com/chromedp/cdproto/page"
	"github.com/chromedp/chromedp"
)

func main() {
	// Valores por defecto
	width := 8.5
	height := 11.0
	landscape := false
	margin := 0.4 // Margen estándar de ~1cm

	// Leer argumentos: ancho alto orientacion margen
	if len(os.Args) >= 5 {
		w, _ := strconv.ParseFloat(os.Args[1], 64)
		h, _ := strconv.ParseFloat(os.Args[2], 64)
		l, _ := strconv.ParseBool(os.Args[3])
		m, _ := strconv.ParseFloat(os.Args[4], 64)
		if w > 0 && h > 0 {
			width = w
			height = h
			landscape = l
			margin = m
		}
	}

	htmlBytes, _ := ioutil.ReadAll(os.Stdin)

	opts := append(chromedp.DefaultExecAllocatorOptions[:],
		chromedp.NoSandbox,
		chromedp.DisableGPU,
	)

	allocCtx, cancel := chromedp.NewExecAllocator(context.Background(), opts...)
	defer cancel()
	ctx, cancel := chromedp.NewContext(allocCtx)
	defer cancel()

	var buf []byte
	err := chromedp.Run(ctx,
		chromedp.Navigate("about:blank"),
		chromedp.ActionFunc(func(ctx context.Context) error {
			frameTree, err := page.GetFrameTree().Do(ctx)
			if err != nil {
				return err
			}
			return page.SetDocumentContent(frameTree.Frame.ID, string(htmlBytes)).Do(ctx)
		}),
		chromedp.ActionFunc(func(ctx context.Context) error {
			var err error
			buf, _, err = page.PrintToPDF().
				WithPrintBackground(true).
				WithPaperWidth(width).
				WithPaperHeight(height).
				WithLandscape(landscape).
				WithMarginTop(margin).    // Margen Superior
				WithMarginBottom(margin). // Margen Inferior
				WithMarginLeft(margin).   // Margen Izquierdo
				WithMarginRight(margin).  // Margen Derecho
				Do(ctx)
			return err
		}),
	)

	if err == nil && len(buf) > 0 {
		os.Stdout.Write(buf)
	}
}