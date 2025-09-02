import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, take, switchMap } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export interface Genre {
  id: string;
  name: string;
  parentGenreId?: string;
}

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {}

  getAllGenres(): Observable<Genre[]> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.genresApiUrl);
        return this.http.get<Genre[]>(url);
      })
    );
  }

  getGenreById(id: string): Observable<Genre> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.genreApiUrl, { id });
    return this.http.get<Genre>(url);
  }

  getGenresByGame(gameKey: string): Observable<Genre[]> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.genresByGameApiUrl, { key: gameKey });
    return this.http.get<Genre[]>(url);
  }

  getGenresByParent(parentId: string): Observable<Genre[]> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.genresByParentApiUrl, { id: parentId });
    return this.http.get<Genre[]>(url);
  }
}
