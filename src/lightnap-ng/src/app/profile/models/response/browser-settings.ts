import { ExtendedSettings, FeaturesSettings, PreferencesSettings } from "../settings";
import { StyleSettings } from "../settings/style-settings";

export interface BrowserSettings {
    extended: ExtendedSettings;
    features: FeaturesSettings;
    preferences: PreferencesSettings;
    style: StyleSettings;
}
