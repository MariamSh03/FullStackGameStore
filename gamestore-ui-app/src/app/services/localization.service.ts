import { Injectable, signal } from '@angular/core';

export interface Language {
  code: string;
  name: string;
}

export interface Translations {
  navbar: {
    games: string;
    genres: string;
    publishers: string;
    orders: string;
    cart: string;
  };
  home: {
    heroTitle: string;
    heroSubtitle: string;
    section1Title: string;
    section1Description: string;
    learnMore: string;
    section2Title: string;
    viewProduct: string;
  };
  games: {
    title: string;
    searchPlaceholder: string;
    loadingGames: string;
    refineResults: string;
    sorting: string;
    sortOptions: {
      popular: string;
      name: string;
      priceLow: string;
      priceHigh: string;
      rating: string;
      newest: string;
    };
    filters: {
      genre: string;
      platform: string;
      priceRange: string;
      publisher: string;
      from: string;
      to: string;
    };
  };
  cart: {
    title: string;
    items: string;
    loadingCart: string;
    errorLoading: string;
    retry: string;
    quantity: string;
    remove: string;
    removing: string;
    orderSummary: string;
    subtotal: string;
    taxes: string;
    total: string;
    checkout: string;
  };
  contact: {
    getInTouch: string;
    name: string;
    email: string;
    message: string;
    submit: string;
    twitterUpdates: string;
    followUs: string;
    fromBlog: string;
    learnMore: string;
    visitBlog: string;
  };
  footer: {
    getInTouch: string;
    emailPlaceholder: string;
    messagePlaceholder: string;
    submit: string;
    twitterUpdates: string;
    followUs: string;
    fromBlog: string;
    learnMore: string;
    visitBlog: string;
    siteLinks: string;
    companyInformation: string;
    socialMedia: string;
    myBasket: string;
    about: string;
    careers: string;
    reviews: string;
    testimonials: string;
    contact: string;
    copyright: string;
    aboutDays: string;
  };
  common: {
    loading: string;
    error: string;
    search: string;
    price: string;
    discount: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class LocalizationService {
  private readonly STORAGE_KEY = 'selectedLanguage';
  
  public readonly availableLanguages: Language[] = [
    { code: 'en', name: 'English'},
    { code: 'ka', name: 'ქართული'},
    { code: 'de', name: 'Deutsch' }
  ];

  private translations: Record<string, Translations> = {
    en: {
      navbar: {
        games: 'GAMES',
        genres: 'GENRES',
        publishers: 'PUBLISHERS',
        orders: 'ORDERS',
        cart: 'CART'
      },
      home: {
        heroTitle: 'What Can Make Your Life Easier?',
        heroSubtitle: 'Empowered Solutions with Simple Process',
        section1Title: 'Do You Know Who is John Rembo?',
        section1Description: 'Lorem ipsum dolor sit amet, consectetur et, adipiscing elit. Curabitur a dignissim lorem.',
        learnMore: 'Learn More',
        section2Title: 'We Knocked Em\'Il',
        viewProduct: 'View Product'
      },
      games: {
        title: 'Games',
        searchPlaceholder: 'Search for Game',
        loadingGames: 'Loading games...',
        refineResults: 'Refine Results',
        sorting: 'Sorting',
        sortOptions: {
          popular: 'Most Popular',
          name: 'Name',
          priceLow: 'Price: Low to High',
          priceHigh: 'Price: High to Low',
          rating: 'Rating',
          newest: 'Newest'
        },
        filters: {
          genre: 'Genre',
          platform: 'Platform',
          priceRange: 'Price Range',
          publisher: 'Publisher',
          from: 'From',
          to: 'To'
        }
      },
      cart: {
        title: 'Cart',
        items: 'items',
        loadingCart: 'Loading cart...',
        errorLoading: 'Error loading cart:',
        retry: 'Retry',
        quantity: 'Quantity',
        remove: 'Remove',
        removing: 'Removing...',
        orderSummary: 'Order Summary',
        subtotal: 'Subtotal',
        taxes: 'Taxes',
        total: 'Total',
        checkout: 'Checkout'
      },
      contact: {
        getInTouch: 'Get In Touch',
        name: 'Name',
        email: 'Email',
        message: 'Message',
        submit: 'Submit',
        twitterUpdates: 'Twitter Updates',
        followUs: 'Follow Us',
        fromBlog: 'From the Blog',
        learnMore: 'Learn more',
        visitBlog: 'Visit Blog'
      },
      footer: {
        getInTouch: 'Get In Touch',
        emailPlaceholder: 'Your email address',
        messagePlaceholder: 'Message',
        submit: 'Submit',
        twitterUpdates: 'Twitter Updates',
        followUs: 'Follow Us',
        fromBlog: 'From the Blog',
        learnMore: 'Learn More',
        visitBlog: 'Visit Blog',
        siteLinks: 'Site Links',
        companyInformation: 'Company Information',
        socialMedia: 'Social Media',
        myBasket: 'My Basket',
        about: 'About',
        careers: 'Careers',
        reviews: 'Reviews',
        testimonials: 'Testimonials',
        contact: 'Contact',
        copyright: '@2017 - 2019 All Rights Reserved',
        aboutDays: 'About'
      },
      common: {
        loading: 'Loading...',
        error: 'Error',
        search: 'Search',
        price: 'Price',
        discount: 'Discount'
      }
    },
    ka: {
      navbar: {
        games: 'თამაშები',
        genres: 'ჟანრები',
        publishers: 'გამომცემლები',
        orders: 'შეკვეთები',
        cart: 'კალათა'
      },
      home: {
        heroTitle: 'რა შეიძლება გახადოს თქვენი ცხოვრება უფრო მარტივი?',
        heroSubtitle: 'გაძლიერებული გადაწყვეტილებები მარტივი პროცესით',
        section1Title: 'იცნობთ ვინ არის ჯონ რემბო?',
        section1Description: 'ლორემ იპსუმ დოლორ სით ამეტ, კონსეკტეტურ ეტ, ადიპისცინგ ელით.',
        learnMore: 'მეტის გაგება',
        section2Title: 'ჩვენ მოვუგეთ',
        viewProduct: 'პროდუქტის ნახვა'
      },
      games: {
        title: 'თამაშები',
        searchPlaceholder: 'თამაშის ძიება',
        loadingGames: 'თამაშები იტვირთება...',
        refineResults: 'შედეგების გაფილტვრა',
        sorting: 'სორტირება',
        sortOptions: {
          popular: 'ყველაზე პოპულარული',
          name: 'სახელი',
          priceLow: 'ფასი: დაბლიდან მაღლამდე',
          priceHigh: 'ფასი: მაღლიდან დაბლამდე',
          rating: 'რეიტინგი',
          newest: 'ყველაზე ახალი'
        },
        filters: {
          genre: 'ჟანრი',
          platform: 'პლატფორმა',
          priceRange: 'ფასების დიაპაზონი',
          publisher: 'გამომცემელი',
          from: 'დან',
          to: 'მდე'
        }
      },
      cart: {
        title: 'კალათა',
        items: 'ელემენტი',
        loadingCart: 'კალათა იტვირთება...',
        errorLoading: 'კალათის ჩატვირთვის შეცდომა:',
        retry: 'თავიდან ცდა',
        quantity: 'რაოდენობა',
        remove: 'წაშლა',
        removing: 'იშლება...',
        orderSummary: 'შეკვეთის შეჯამება',
        subtotal: 'ქვეჯამი',
        taxes: 'გადასახადები',
        total: 'სულ',
        checkout: 'გადახდა'
      },
      contact: {
        getInTouch: 'დაგვიკავშირდით',
        name: 'სახელი',
        email: 'ელ-ფოსტა',
        message: 'შეტყობინება',
        submit: 'გაგზავნა',
        twitterUpdates: 'Twitter განახლებები',
        followUs: 'გამოგვყევით',
        fromBlog: 'ბლოგიდან',
        learnMore: 'მეტის გაგება',
        visitBlog: 'ბლოგის ნახვა'
      },
      footer: {
        getInTouch: 'დაგვიკავშირდით',
        emailPlaceholder: 'თქვენი ელ-ფოსტის მისამართი',
        messagePlaceholder: 'შეტყობინება',
        submit: 'გაგზავნა',
        twitterUpdates: 'Twitter განახლებები',
        followUs: 'გამოგვყევით',
        fromBlog: 'ბლოგიდან',
        learnMore: 'მეტის გაგება',
        visitBlog: 'ბლოგის ნახვა',
        siteLinks: 'საიტის ბმულები',
        companyInformation: 'კომპანიის ინფორმაცია',
        socialMedia: 'სოციალური მედია',
        myBasket: 'ჩემი კალათა',
        about: 'ჩვენ შესახებ',
        careers: 'კარიერა',
        reviews: 'მიმოხილვები',
        testimonials: 'რეკომენდაციები',
        contact: 'კონტაქტი',
        copyright: '@2017 - 2019 ყველა უფლება დაცულია',
        aboutDays: 'დაახლოებით'
      },
      common: {
        loading: 'იტვირთება...',
        error: 'შეცდომა',
        search: 'ძიება',
        price: 'ფასი',
        discount: 'ფასდაკლება'
      }
    },
    de: {
      navbar: {
        games: 'SPIELE',
        genres: 'GENRES',
        publishers: 'VERLAGE',
        orders: 'BESTELLUNGEN',
        cart: 'WARENKORB'
      },
      home: {
        heroTitle: 'Was kann Ihr Leben einfacher machen?',
        heroSubtitle: 'Leistungsstarke Lösungen mit einfachem Prozess',
        section1Title: 'Wissen Sie, wer John Rembo ist?',
        section1Description: 'Lorem ipsum dolor sit amet, consectetur et, adipiscing elit. Curabitur a dignissim lorem.',
        learnMore: 'Mehr erfahren',
        section2Title: 'Wir haben sie geschlagen',
        viewProduct: 'Produkt ansehen'
      },
      games: {
        title: 'Spiele',
        searchPlaceholder: 'Nach Spiel suchen',
        loadingGames: 'Spiele werden geladen...',
        refineResults: 'Ergebnisse verfeinern',
        sorting: 'Sortierung',
        sortOptions: {
          popular: 'Beliebteste',
          name: 'Name',
          priceLow: 'Preis: Niedrig bis Hoch',
          priceHigh: 'Preis: Hoch bis Niedrig',
          rating: 'Bewertung',
          newest: 'Neueste'
        },
        filters: {
          genre: 'Genre',
          platform: 'Plattform',
          priceRange: 'Preisbereich',
          publisher: 'Verlag',
          from: 'Von',
          to: 'Bis'
        }
      },
      cart: {
        title: 'Warenkorb',
        items: 'Artikel',
        loadingCart: 'Warenkorb wird geladen...',
        errorLoading: 'Fehler beim Laden des Warenkorbs:',
        retry: 'Wiederholen',
        quantity: 'Menge',
        remove: 'Entfernen',
        removing: 'Wird entfernt...',
        orderSummary: 'Bestellübersicht',
        subtotal: 'Zwischensumme',
        taxes: 'Steuern',
        total: 'Gesamt',
        checkout: 'Zur Kasse'
      },
      contact: {
        getInTouch: 'Kontakt aufnehmen',
        name: 'Name',
        email: 'E-Mail',
        message: 'Nachricht',
        submit: 'Senden',
        twitterUpdates: 'Twitter Updates',
        followUs: 'Folgen Sie uns',
        fromBlog: 'Vom Blog',
        learnMore: 'Mehr erfahren',
        visitBlog: 'Blog besuchen'
      },
      footer: {
        getInTouch: 'Kontakt aufnehmen',
        emailPlaceholder: 'Ihre E-Mail-Adresse',
        messagePlaceholder: 'Nachricht',
        submit: 'Senden',
        twitterUpdates: 'Twitter Updates',
        followUs: 'Folgen Sie uns',
        fromBlog: 'Vom Blog',
        learnMore: 'Mehr erfahren',
        visitBlog: 'Blog besuchen',
        siteLinks: 'Site-Links',
        companyInformation: 'Firmeninformationen',
        socialMedia: 'Soziale Medien',
        myBasket: 'Mein Warenkorb',
        about: 'Über uns',
        careers: 'Karriere',
        reviews: 'Bewertungen',
        testimonials: 'Testimonials',
        contact: 'Kontakt',
        copyright: '@2017 - 2019 Alle Rechte vorbehalten',
        aboutDays: 'Vor etwa'
      },
      common: {
        loading: 'Wird geladen...',
        error: 'Fehler',
        search: 'Suchen',
        price: 'Preis',
        discount: 'Rabatt'
      }
    }
  };

  // Signal for reactive language changes
  public currentLanguage = signal<string>(this.getStoredLanguage());

  constructor() {
    // Set initial language
    this.setLanguage(this.currentLanguage());
  }

  private getStoredLanguage(): string {
    if (typeof window !== 'undefined') {
      return localStorage.getItem(this.STORAGE_KEY) || 'en';
    }
    return 'en';
  }

  public setLanguage(languageCode: string): void {
    if (this.translations[languageCode]) {
      this.currentLanguage.set(languageCode);
      if (typeof window !== 'undefined') {
        localStorage.setItem(this.STORAGE_KEY, languageCode);
      }
    }
  }

  public getTranslation(key: string): string {
    const currentLang = this.currentLanguage();
    const keys = key.split('.');
    let translation: any = this.translations[currentLang];
    
    for (const k of keys) {
      if (translation && typeof translation === 'object') {
        translation = translation[k];
      } else {
        // Fallback to English if translation not found
        translation = this.translations['en'];
        for (const fallbackKey of keys) {
          if (translation && typeof translation === 'object') {
            translation = translation[fallbackKey];
          } else {
            return key; // Return key as fallback
          }
        }
        break;
      }
    }
    
    return typeof translation === 'string' ? translation : key;
  }

  public getCurrentLanguage(): Language {
    return this.availableLanguages.find(lang => lang.code === this.currentLanguage()) || this.availableLanguages[0];
  }
}
