/**
 * Represents the style settings for the application.
 */
export interface StyleSettings {
    /**
     * The scale factor for the UI elements.
     */
    scale: number;

    /**
     * The mode of the menu.
     */
    menuMode: string;

    /**
     * The style of the input elements.
     */
    inputStyle: string;

    /**
     * Indicates whether ripple effect is enabled.
     */
    ripple: boolean;

    /**
     * The theme of the application.
     */
    theme: string;

    /**
     * The color scheme of the application.
     */
    colorScheme: string;
}
